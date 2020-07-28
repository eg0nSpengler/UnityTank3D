using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton to manage the current GameState and related logic
/// </summary>
/// 

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [Header("Audio References")]
    public AudioClip PickupScoreSound;
    public AudioClip TimeScoreSound;
    public AudioClip TimeScoreEndSound;
    public AudioClip GuitarRiffSound;
    public AudioClip PauseSound;

    /// <summary>
    /// Is the game currently paused?
    /// </summary>
    public static bool IsGamePaused { private set; get; }

    private AudioSource _audioSource;


    public delegate void GameStateMenu();
    public delegate void GameStatePreBrief();
    public delegate void GameStatePlay();
    public delegate void GameStatePostBrief();
    public delegate void GameStateGameOver();
    public delegate void GamePause();
    public delegate void GameResume();
    public delegate int GameLoadLevel();

    /// <summary>
    /// Called when the GameState is MENU (Player is in the Main Menu scene)
    /// </summary>
    public static event GameStateMenu OnGameStateMenu;

    /// <summary>
    /// Called when the GameState is PREBRIEFING (Player is in the Pre-Briefing scene)
    /// </summary>
    public static event GameStatePreBrief OnGameStatePreBrief;

    /// <summary>
    /// Called when the GameState is PLAY (Player is playing a level)
    /// </summary>
    public static event GameStatePlay OnGameStatePlayEvent;

    /// <summary>
    /// Called when the GameState is POSTBRIEFING (Player is the post-briefing scene)
    /// </summary>
    public static event GameStatePostBrief OnGameStatePostBrief;

    /// <summary>
    /// Called when the GameState is GAME OVER (Player has died or ran out of time)
    /// </summary>
    public static event GameStateGameOver OnGameStateGameOver;

    /// <summary>
    /// Called when the user presses ESC during Gameplay (this will open the pause menu)
    /// </summary>
    public static event GamePause OnGamePause;

    /// <summary>
    /// Called when the user presses ESC while at the Pause Menu (this will close the pause menu and resume the game)
    /// </summary>
    public static event GameResume OnGameResume;

    public enum GAME_STATE 
    {
        STATE_NONE,
        STATE_MENU,
        STATE_PREBRIEFING,
        STATE_PLAYING,
        STATE_POSTBRIEFING,
        STATE_GAME_OVER
    }

    /// <summary>
    /// The current GameState
    /// </summary>
    public static GAME_STATE GameState { private set; get; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (!_audioSource)
        {
            Debug.LogError("Failed to get AudioSource on GameManager, creating one now");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!PickupScoreSound || !TimeScoreSound || !TimeScoreEndSound || !GuitarRiffSound || !PauseSound)
        {
            Debug.LogWarning("GameManager is missing a sound reference!");
        }

        if (FindObjectOfType<LevelPortal>() != null)
        {
            FindObjectOfType<LevelPortal>().OnPlayerEnterPortalEvent += PauseGame;
        }
        //PickupManager.OnAllPickupsCollectedEvent += SaveData;

        IsGamePaused = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Are we at the main menu?
        if (SceneManager.GetActiveScene().name.Contains("MENU"))
        {
            // We're at the main menu
            GameState = GAME_STATE.STATE_MENU;
            GameDataSerializer.InitGameData();
        } 
        else if(GameState == GAME_STATE.STATE_PLAYING) // Are we currently playing a level?
        {
            // Let's listen and wait for a pause request then
            StartCoroutine(WaitForPauseState());
        }

        Debug.Log(GameState.ToString());
    }

    private void OnEnable()
    {
        MenuRollingDoor.OnGameStartEvent += StartGame;
        StartLevelButton.OnLevelStartEvent += PlayGame;
        MobManager.OnPlayerMobDestroyed += EndGame;
        LevelTimerBox.OnLevelTimerEnd += EndGame;
        ExitToMenuButton.OnExitToMenuRequest += MenuGame;
        PickupDisplayBox.OnPickupScoreUpdate += PlayScoreAudio;
        LevelTimerBox.OnLevelTimerScoreUpdate += PlayTimeScoreAudio;
        LevelTimerBox.OnLevelTimerScoreEnd += EndTimeScoreAudio;
        LevelTimerBox.OnLevelTimerScoreEnd += PlayNextLevel;
        OnGamePause += PlayPauseAudio;
    }

    private void OnDisable()
    {
        MenuRollingDoor.OnGameStartEvent -= StartGame;
        StartLevelButton.OnLevelStartEvent -= PlayGame;
        MobManager.OnPlayerMobDestroyed -= EndGame;
        LevelTimerBox.OnLevelTimerEnd -= EndGame;
        ExitToMenuButton.OnExitToMenuRequest -= MenuGame;
        PickupDisplayBox.OnPickupScoreUpdate -= PlayScoreAudio;
        LevelTimerBox.OnLevelTimerScoreUpdate -= PlayTimeScoreAudio;
        LevelTimerBox.OnLevelTimerScoreEnd -= EndTimeScoreAudio;
        LevelTimerBox.OnLevelTimerScoreEnd -= PlayNextLevel;
        OnGamePause -= PlayPauseAudio;


        if (FindObjectOfType<LevelPortal>() != null)
        {
            FindObjectOfType<LevelPortal>().OnPlayerEnterPortalEvent -= PauseGame;
        }
        //PickupManager.OnAllPickupsCollectedEvent -= SaveData;

        StopAllCoroutines();
    }

    private void MenuGame()
    {
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_MENU);
        GameState = GAME_STATE.STATE_MENU;
        OnGameStateMenu?.Invoke();

    }
    private void StartGame()
    {
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_PRE_BRIEFING);
        GameState = GAME_STATE.STATE_PREBRIEFING;
        OnGameStatePreBrief?.Invoke();
    }

    private void PlayGame()
    {
        //This just checks to see if we're at the main menu
        if (SceneManager.GetActiveScene().name.Contains("MENU"))
        {
            // We're at the main menu, load the pre-briefing scene
            StartGame();
            return;
        }
        else
        {
            LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_PLAY);
            GameState = GAME_STATE.STATE_PLAYING;
            OnGameStatePlayEvent?.Invoke();    
        }
    }

    private void PauseGame()
    {
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_POST_BRIEFING);
        GameState = GAME_STATE.STATE_POSTBRIEFING;
        OnGameStatePostBrief?.Invoke();
    }


    void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
        OnGameStateGameOver?.Invoke();
    }

    void PlayNextLevel()
    {
        StartCoroutine(PlayNextLevelCoroutine());
    }

    IEnumerator EndGameCoroutine()
    {
        Debug.Log("Game over!");

        GameState = GAME_STATE.STATE_GAME_OVER;

        //We wait for the user to press space, once pressed the level will restart
        yield return new WaitWhile(() => Input.GetKeyDown(KeyCode.Space) == false);

        Debug.Log("Restarting game!");

        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_PRE_BRIEFING);
    }

    IEnumerator PlayNextLevelCoroutine()
    {
        Debug.Log("Waiting for user to press space to load next level");

        // Waiting for the user to press space
        yield return new WaitWhile(() => Input.GetKeyDown(KeyCode.Space) == false);

        PlayGuitarRiff();

        // Just to make sure the audio plays all the way through
        yield return new WaitWhile(() => _audioSource.isPlaying == true);

        Debug.Log("Loading next level...");

        StartGame();
    }

    IEnumerator WaitForPauseState()
    {
        while (!Input.GetKeyDown(KeyCode.Escape))
        {
            yield return null;
        }

        Debug.Log("GAME PAUSED!");
        IsGamePaused = true;
        OnGamePause?.Invoke();
        yield return new WaitForSeconds(0.5f); // BEGONE RACE CONDITION
        StartCoroutine(WaitForUnPauseState());
    }

    IEnumerator WaitForUnPauseState()
    {
        while (!Input.GetKeyDown(KeyCode.Escape))
        {
            yield return null;
        }

        Debug.Log("GAME RESUMED!");
        IsGamePaused = false;
        OnGameResume?.Invoke();
        yield return new WaitForSeconds(0.5f); // BEGONE RACE CONDITION
        StartCoroutine(WaitForPauseState());
    }

    void PlayScoreAudio()
    {
        _audioSource.clip = PickupScoreSound;
        _audioSource.Play();
    }

    void PlayTimeScoreAudio()
    {
        _audioSource.clip = TimeScoreSound;
        _audioSource.Play();
    }

    void EndTimeScoreAudio()
    {
        _audioSource.clip = TimeScoreEndSound;
        _audioSource.Play();
    }

    void PlayGuitarRiff()
    {
        _audioSource.clip = GuitarRiffSound;
        _audioSource.Play();
    }

    void PlayPauseAudio()
    {
        _audioSource.clip = PauseSound;
        _audioSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelTimerBox : MonoBehaviour
{
    [Header("GameObject References")]
    public GameObject thisGameObject;

    public static int FinalLevelTime { get; private set; }

    private TextMeshProUGUI _levelTimeText;
    private float _levelTime;
    private bool IsCalled;

    public delegate void LevelTimerLow();
    public delegate void LevelTimerEnd();
    public delegate void LevelTimeScoreBegin();
    public delegate void LevelTimeScoreUpdate();
    public delegate void LevelTimerScoreEnd();

    /// <summary>
    /// Called when the Level Timer reaches the end of its countdown
    /// </summary>
    public static event LevelTimerEnd OnLevelTimerEnd;

    /// <summary>
    /// Called when there is 10 seconds remaining on the Level Timer
    /// </summary>
    public static event LevelTimerLow OnLevelTimerLow;

    /// <summary>
    /// Called when the player score begins to update from the Level Time
    /// </summary>
    public static event LevelTimeScoreBegin OnLevelTimerScoreBegin;

    /// <summary>
    /// Called when the player score is updated from the Level Time
    /// </summary>
    public static event LevelTimeScoreUpdate OnLevelTimerScoreUpdate;

    /// <summary>
    /// Called when the player score has finished updating from the Level Time
    /// </summary>
    public static event LevelTimerScoreEnd OnLevelTimerScoreEnd;

    private void Awake()
    {
        _levelTimeText = GetComponent<TextMeshProUGUI>();
        _levelTimeText.color = Color.green;
        IsCalled = false;

        if (!_levelTimeText)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _levelTimeText = gameObject.AddComponent<TextMeshProUGUI>();
        }

        SceneManager.sceneLoaded += Init;
        GameManager.OnGamePause += StopTimer;
        GameManager.OnGameResume += ResumeTimer;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager.CurrentLevelStats != null)
        {
            if (LevelManager.CurrentLevelStats.CurrentLevelTime > 0)
            {
                _levelTime = LevelManager.CurrentLevelStats.CurrentLevelTime;
            }
            else
            {
                _levelTime = 999f;
            }

            StartCoroutine(CheckLevelTime());
        }

        FindObjectOfType<LevelPortal>().OnPlayerEnterPortalEvent += SaveTime;
    }

    private void Init(Scene scene, LoadSceneMode mode)
    {
        
        if (_levelTimeText == null)
        {
            Debug.LogError("Level Timer Box is null!");
            _levelTimeText = GetComponent<TextMeshProUGUI>();
        }
        
    }

    private void OnEnable()
    {
        Debug.Log("LevelTimerbox enabled at " + Time.time.ToString());
    }

    private void OnDisable()
    {
        FindObjectOfType<LevelPortal>().OnPlayerEnterPortalEvent -= SaveTime;
        SceneManager.sceneLoaded -= Init;
        GameManager.OnGamePause -= StopTimer;
        GameManager.OnGameResume -= ResumeTimer;

        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        Debug.Log("LevelTimerbox destroyed at " + Time.time.ToString());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameState == GameManager.GAME_STATE.STATE_PLAYING || GameManager.GameState == GameManager.GAME_STATE.STATE_POSTBRIEFING)
        {
            if (GameManager.IsGamePaused == false)
            {
                UpdateTime();
            }
        }

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            TenSecLevelTime();
        }

    }

    /// <summary>
    /// This is to play the alarm sound when the time remaining in a level is <= 10
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckLevelTime()
    {
        //We just wait until the time remaining is <= 10
        yield return new WaitUntil(() => _levelTime < 11);
        
        // We play the alarm sound every second
        InvokeRepeating("InvokeTimerEvent", 0.1f, 1f);
        _levelTimeText.color = Color.red;

        //We wait until the time is <= 0
        yield return new WaitUntil(() => _levelTime <= 0);
        
        //Out of time, stop playing the alarm sound.
        OnLevelTimerEnd?.Invoke();
        CancelInvoke();
    }

    void InvokeTimerEvent()
    {
        OnLevelTimerLow?.Invoke();
    }

    void UpdateTime()
    {
        if (_levelTime > 0 && GameManager.GameState == GameManager.GAME_STATE.STATE_PLAYING)
        {
            _levelTime -= Time.deltaTime;
            var lvlTime = Mathf.RoundToInt(_levelTime);
            _levelTimeText.text = lvlTime.ToString();
        }

        if (GameManager.GameState == GameManager.GAME_STATE.STATE_POSTBRIEFING)
        {
            _levelTimeText.text = FinalLevelTime.ToString();
        }
        else
        {
            _levelTime -= Time.deltaTime;
            var lvlTime = Mathf.RoundToInt(_levelTime);
            _levelTimeText.text = lvlTime.ToString();
        }
    }

    void SaveTime()
    {
        FinalLevelTime = Mathf.RoundToInt(_levelTime);
    }

    public static IEnumerator HandleTimeScore()
    {
        OnLevelTimerScoreBegin?.Invoke();

        yield return new WaitForSeconds(0.5f);
        var timeMultipler = 100;
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        while (FinalLevelTime > 0)
        {
            yield return new WaitForSeconds(0.01f);
            FinalLevelTime -= 1;
            gmData.playerScore += timeMultipler;
            OnLevelTimerScoreUpdate?.Invoke();
        }

        OnLevelTimerScoreEnd?.Invoke();
        Debug.Log("Invoking LevelTimerEnd!");

    }

    /// <summary>
    /// A cheat method to set the level time to 10 seconds
    /// </summary>
    private void TenSecLevelTime()
    {
        if (_levelTime > 10)
        {
            _levelTime = 10;
        }
    }

    private void StopTimer()
    {
        
    }

    private void ResumeTimer()
    {

    }
}

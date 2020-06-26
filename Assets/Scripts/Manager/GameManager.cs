using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton to manage the current GameState and related logic
/// </summary>
public class GameManager : MonoBehaviour
{
    
    public delegate void GameStatePlay();
    public delegate void GameStatePostBrief();
    public delegate int GameLoadLevel();

    /// <summary>
    /// Called when the GameState is PLAY
    /// </summary>
    public static event GameStatePlay OnGameStatePlayEvent;

    /// <summary>
    /// Called when the GameState is POSTBRIEFING
    /// </summary>
    public static event GameStatePostBrief OnGameStatePostBrief;

    public enum GAME_STATE 
    {
        STATE_NONE,
        STATE_MENU,
        STATE_PREBRIEFING,
        STATE_PLAYING,
        STATE_POSTBRIEFING,
        STATE_GAME_OVER
    }

    private static GAME_STATE _gameState;


    private void Awake()
    {
        MenuRollingDoor.OnGameStartEvent += StartGame;
        StartLevelButton.OnLevelStartEvent += PlayGame;
        LevelPortal.OnPlayerEnterPortalEvent += PauseGame;
        LevelManager.OnLevelTimerEnd += PauseGame;
        //PickupManager.OnAllPickupsCollectedEvent += SaveData;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void OnDisable()
    {
        MenuRollingDoor.OnGameStartEvent -= StartGame;
        StartLevelButton.OnLevelStartEvent -= PlayGame;
        LevelPortal.OnPlayerEnterPortalEvent -= PauseGame;
        LevelManager.OnLevelTimerEnd -= PauseGame;
        //PickupManager.OnAllPickupsCollectedEvent -= SaveData;
    }
    /// <summary>
    /// Returns the current GameState
    /// </summary>
    /// <returns>GAME_STATE</returns>
    public static GAME_STATE GetGameState()
    {
        return _gameState;
    }

    private void StartGame()
    {
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_PRE_BRIEFING);
        _gameState = GAME_STATE.STATE_PREBRIEFING;
    }

    private void PlayGame()
    {
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_PLAY);
        _gameState = GAME_STATE.STATE_PLAYING;
        OnGameStatePlayEvent();
    }

    private void PauseGame()
    {
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_POST_BRIEFING);
        _gameState = GAME_STATE.STATE_POSTBRIEFING;
        OnGameStatePostBrief();
    }

}

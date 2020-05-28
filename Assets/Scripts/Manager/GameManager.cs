using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton to manage the current GameState and related logic
/// </summary>
public class GameManager : MonoBehaviour
{
    

    [Header("References")]
    public GameObject LevelPortal;


    public delegate void GameStatePlay();

    /// <summary>
    /// Called when the GameState is PLAY
    /// </summary>
    public static event GameStatePlay OnGameStatePlayEvent;

    private static GAME_STATE _gameState;

    public enum GAME_STATE 
    {
        STATE_NONE,
        STATE_MENU,
        STATE_PREBRIEFING,
        STATE_PLAYING,
        STATE_POSTBRIEFING,
        STATE_GAME_OVER
    }

    private void Awake()
    {
        _gameState = GAME_STATE.STATE_MENU;
        MenuRollingDoor.OnGameStartEvent += StartGame;
        StartLevelButton.OnLevelStartEvent += PlayGame;
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    private void OnDisable()
    {
        MenuRollingDoor.OnGameStartEvent -= StartGame;
        StartLevelButton.OnLevelStartEvent -= PlayGame;
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
        Debug.Log("StartGame called from GameManager");
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_PRE_BRIEFING);
        _gameState = GAME_STATE.STATE_PREBRIEFING;
    }

    private void PlayGame()
    {
        LevelManager.LoadLevel(LevelManager.LEVEL_TYPE.LEVEL_PLAY);
        _gameState = GAME_STATE.STATE_PLAYING;
        OnGameStatePlayEvent();
    }

}

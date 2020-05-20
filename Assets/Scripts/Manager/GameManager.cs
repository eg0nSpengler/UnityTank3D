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


    private static GAME_STATE _gameState;

    public enum GAME_STATE 
    {
        STATE_NONE,
        STATE_PREBRIEFING,
        STATE_PLAYING,
        STATE_POSTBRIEFING,
        STATE_GAME_OVER
    }

    private void Awake()
    {
        
    }

    private void OnEnable()
    {

    }
    // Start is called before the first frame update
    private void Start()
    {
        _gameState = GAME_STATE.STATE_PREBRIEFING;
    }

    /// <summary>
    /// Returns the current GameState
    /// </summary>
    /// <returns>GAME_STATE</returns>
    public static GAME_STATE GetGameState()
    {
        return _gameState;
    }

}

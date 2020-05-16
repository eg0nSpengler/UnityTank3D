using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton to manage the current GameState and related logic
/// </summary>
public class GameManager : MonoBehaviour
{
    
    private enum GAME_STATE 
    {
        STATE_NONE,
        STATE_PREBRIEFING,
        STATE_PLAYING,
        STATE_POSTBRIEFING,
        STATE_GAME_OVER
    }

    [Header("References")]
    public GameObject LevelPortal;

    private static OnPlayerEnterPortal OnPlayerEnterPortalDelegate;
    private static OnPreBriefingLoad OnPreBriefingLoadDelegate;

    private static GAME_STATE _gameState;
     
    private int _playerScore = 0;

    private delegate void OnPlayerEnterPortal();
    private delegate void OnPreBriefingLoad();

    private void Awake()
    {
        OnPlayerEnterPortalDelegate = LoadPostBriefingScene;
        OnPreBriefingLoadDelegate = LoadPreBriefingScene;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Start is called before the first frame update
    private void Start()
    {
        _gameState = GAME_STATE.STATE_PREBRIEFING;
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }


    private void ShowPortal()
    {
        LevelPortal.SetActive(true);
    }

    private static void LoadPreBriefingScene()
    {
        Debug.Log("Loading PRE_BRIEFING SCENE");
        SceneManager.LoadSceneAsync("PRE_BRIEFING", LoadSceneMode.Single);
    }
    private static void LoadPostBriefingScene()
    {
        Debug.Log("Loading POST_BRIEFING SCENE");
        SceneManager.LoadSceneAsync("POST_BRIEFING", LoadSceneMode.Single);
    }


    public static void InvokeOnPlayerEnterPortalDelegate(GameObject obj)
    {
        if (obj.tag == "LevelPortal")
        {
            OnPlayerEnterPortalDelegate.Invoke();
        }
        else
        {
            Debug.LogError("Failed to invoke OnPlayerEnterPortal Delegate in the Game Manager, you probably tried to invoke this delegate from the wrong GameObject");
        }
    }

    public static void InvokePreBriefingDelegate(GameObject obj)
    {
        if (obj.tag == "Interactables")
        {
            OnPreBriefingLoadDelegate.Invoke();
        }
        else
        {
            Debug.LogError("Failed to invoke PreBriefing Delegate in the Game Manager, you probably tried to invoke this delegate from the wrong GameObject");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }

}

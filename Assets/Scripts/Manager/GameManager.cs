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
     
    private int _playerScore = 0;

    private delegate void OnPlayerEnterPortal();
    private enum GAME_STATE 
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

    // Update is called once per frame
    private void Update()
    {
        
    }


    private void ShowPortal()
    {
        LevelPortal.SetActive(true);
    }

    public static void InvokeOnPlayerEnterPortalDelegate(GameObject obj)
    {
        if (obj.tag == "LevelPortal")
        {
            LevelManager.LoadScene(LevelManager.BriefingType.POST_BRIEFING);
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
            LevelManager.LoadScene(LevelManager.BriefingType.PRE_BRIEFING);
        }
        else
        {
            Debug.LogError("Failed to invoke PreBriefing Delegate in the Game Manager, you probably tried to invoke this delegate from the wrong GameObject");
        }
    }

}

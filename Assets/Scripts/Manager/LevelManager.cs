using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton that manages the loading/unloading of levels and any logic pertaining to the level state (Level Name/Time/ etc)
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelInfo levelInfo;

    public enum LEVEL_TYPE
    {
        LEVEL_NONE,
        LEVEL_PRE_BRIEFING,
        LEVEL_POST_BRIEFING,
        LEVEL_PLAY,
    }

    private static int _levelNum;
    private static float _levelTime;

    private static List<string> _sceneList;



    private void Awake()
    {
        _sceneList = new List<string>();

        var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            var str = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            _sceneList.Add(str);
            Debug.Log("Added " + str + " to SceneList in LevelManager");
        }

        if (levelInfo)
        {
            _levelNum = levelInfo.GetLevelNum;
            _levelTime = levelInfo.GetLevelTime;
        }
        else
        {
            Debug.LogWarning("No LevelInfo provided for LevelManager...");
            _levelTime = 0.0f;
            _levelNum = 0;
        }

        Debug.Log("The SceneList in LevelManager contains " + _sceneList.Count.ToString() + " scene(s)");

        LevelPortal.OnPlayerEnterPortalEvent += LoadPostBriefing;
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameManager.GetGameState() == GameManager.GAME_STATE.STATE_PLAYING)
        {
            _levelTime -= Time.deltaTime;
        }
        
    }

    public static void LoadLevel(LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case LEVEL_TYPE.LEVEL_NONE:
                Debug.LogError("No level passed in for LoadLevel");
                break;

            case LEVEL_TYPE.LEVEL_PLAY:
                SceneManager.LoadSceneAsync("TestScene");
                break;

            case LEVEL_TYPE.LEVEL_POST_BRIEFING:
                SceneManager.LoadSceneAsync("POST_BRIEFING");
                break;

            case LEVEL_TYPE.LEVEL_PRE_BRIEFING:
                SceneManager.LoadSceneAsync("PRE_BRIEFING");
                break;

            default:
                break;
        }
    }

    private void LoadPostBriefing()
    {
        SceneManager.LoadSceneAsync("POST_BRIEFING");
    }

    /// <summary>
    /// Returns the current level number
    /// </summary>
    /// <returns></returns>
    public static int GetLevelNum()
    {
        return _levelNum;
    }

    /// <summary>
    /// Returns the current level time
    /// </summary>
    /// <returns></returns>
    public static int GetLevelTime()
    {
        return Mathf.RoundToInt(_levelTime);
    }

}

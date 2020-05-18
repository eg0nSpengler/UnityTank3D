using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A singleton that manages the loading/unloading of levels(Didn't refer to this class as SceneManager because such a class already exists)
/// </summary>
public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelInfo levelInfo;

    private static int _levelNum;
    private static int _levelTime;

    public enum BriefingType
    {
        PRE_BRIEFING,
        POST_BRIEFING,
    }

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

        _levelNum = levelInfo.GetLevelNum;
        _levelTime = levelInfo.GetLevelTime;

        Debug.Log("The SceneList in LevelManager contains " + _sceneList.Count.ToString() + " scene(s)");

    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public static void LoadScene(BriefingType briefType)
    {
        var preBrief = "PRE_BRIEFING";
        var postBrief = "POST_BRIEFING";

        switch (briefType)
        {
            case BriefingType.PRE_BRIEFING:
                SceneManager.LoadSceneAsync(preBrief, LoadSceneMode.Single);
                Debug.Log("Loaded " + preBrief + " from LevelManager");
                break;

            case BriefingType.POST_BRIEFING:
                SceneManager.LoadSceneAsync(postBrief, LoadSceneMode.Single);
                Debug.Log("Loaded " + postBrief + " from LevelManager");
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Returns the current loaded Level Number
    /// </summary>
    /// <returns></returns>
    public static int GetLevelNum()
    {
        return _levelNum;
    }

    public static int GetLevelTime()
    {
        return _levelTime;
    }

}

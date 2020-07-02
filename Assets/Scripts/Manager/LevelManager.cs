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

    [Header("Audio References")]
    public AudioClip lowTimeSound;

    public enum LEVEL_TYPE
    {
        LEVEL_NONE,
        LEVEL_PRE_BRIEFING,
        LEVEL_POST_BRIEFING,
        LEVEL_PLAY,
    }

    [SerializeField]
    public class LevelStats
    {
        private static int _currentLevelNum;
        private static float _currentLevelTime;

        /// <summary>
        /// The current level number
        /// </summary>
        public int CurrentLevelNum
        {
            get => _currentLevelNum;
            set => _currentLevelNum = value;
        }
        
        /// <summary>
        /// The current level time
        /// </summary>
        public float CurrentLevelTime
        {
            get => _currentLevelTime;            
            set => _currentLevelTime = value;
        }
        
    }

    public delegate void LevelTimerEnd();

    /// <summary>
    /// Called when the Level Timer reaches the end of its countdown
    /// </summary>
    public static event LevelTimerEnd OnLevelTimerEnd;

    private AudioSource _audioSource;

    private static int _levelIter;

    private static List<string> _sceneList;
    private static List<string> _levelList;

    /// <summary>
    /// The current stats for the loaded level
    /// </summary>
    public static LevelStats CurrentLevelStats { private set; get; }



    private void Awake()
    {
        _sceneList = new List<string>();
        _levelList = new List<string>();
        _audioSource = GetComponent<AudioSource>();

        CurrentLevelStats = new LevelStats();

        var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            var str = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            _sceneList.Add(str);
            if (str.Contains("LEVEL"))
            {
                _levelList.Add(str);
                Debug.Log("Added " + str + " to LevelList");
            }

            Debug.Log("Added " + str + " to SceneList in LevelManager");
        }

        if (levelInfo)
        {
            CurrentLevelStats.CurrentLevelNum = levelInfo.GetLevelNum;
            CurrentLevelStats.CurrentLevelTime = levelInfo.GetLevelTime;
        }
        else
        {
            Debug.LogWarning("No LevelInfo provided for LevelManager...");
            CurrentLevelStats.CurrentLevelNum = 0;
            CurrentLevelStats.CurrentLevelTime = 0;
        }

        if (!lowTimeSound)
        {
            Debug.LogWarning("No Low Time Sound Provided for LevelManager!");
        }

        if (!_audioSource)
        {
            Debug.LogError("No AudioSource found on LevelManager!" + ", creating one now.");
            gameObject.AddComponent<AudioSource>();
        }

        _levelIter = 0;
        _audioSource.clip = lowTimeSound;
        Debug.Log("The SceneList in LevelManager contains " + _sceneList.Count.ToString() + " scene(s)");
        Debug.Log("The LevelList in LevelManager contains " + _levelList.Count.ToString() + " level(s)");
        
        LevelPortal.OnPlayerEnterPortalEvent += LoadPostBriefing;
        GameManager.OnGameStatePlayEvent += InvokeLevelTimer;
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void OnDisable()
    {
        LevelPortal.OnPlayerEnterPortalEvent -= LoadPostBriefing;
        GameManager.OnGameStatePlayEvent -= InvokeLevelTimer;
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


    private void SaveLevelInfo()
    {
        CurrentLevelStats.CurrentLevelNum = levelInfo.GetLevelNum;
        
    }

    private void LoadPostBriefing()
    {
        _levelIter++;
        SceneManager.LoadSceneAsync("POST_BRIEFING");
        Debug.Log("Level Iterator is now " + _levelIter.ToString());
    }


    private void InvokeLevelTimer()
    {
        
    }
    void RunLevelTimer()
    {
        Debug.Log("RunLevelTimer Coroutine ran!");
        
    }

    
}

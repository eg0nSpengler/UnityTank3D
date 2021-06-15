using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

/// <summary>
/// A singleton that manages the loading/unloading of levels and any logic pertaining to the level state (Level Name/Time/ etc)
/// </summary>
/// 

[RequireComponent(typeof(LevelInfoObject))]
[RequireComponent(typeof(AudioSource))]
public class LevelManager : MonoBehaviour
{
    [Header("References")]
    public LevelInfo levelInfo;
    public LevelInfoObject levelInfoList;

    [Header("Audio References")]
    public AudioClip lowTimeSound;

    public delegate void ValidLevelFound();
    public delegate void NoValidLevelFound();

    /// <summary>
    /// Called when the LevelManager finds a valid level within the LevelList
    /// <para>This event is primarily meant for use with classes that request/provide input for level related logic(such as the PlayLevelButton class)</para>
    /// </summary>
    public static event ValidLevelFound OnValidLevelFound;

    /// <summary>
    /// Called when the LevelManager fails to find a valid level within the LevelList
    /// <para>This event is primarily meant for use with classes that request/provide input for level related logic(such as the PlayLevelButton class)</para>
    /// </summary>
    public static event NoValidLevelFound OnNoValidLevelFound;

    public enum LEVEL_TYPE
    {
        /// <summary>
        /// No scene loaded
        /// </summary>
        LEVEL_NONE,
        /// <summary>
        /// Main Menu
        /// </summary>
        LEVEL_MENU,
        /// <summary>
        /// Pre briefing screen
        /// <para> Where the user reads the level title and description</para>
        /// </summary>
        LEVEL_PRE_BRIEFING,
        /// <summary>
        /// Post briefing screen (Scoring screen)
        /// </summary>
        LEVEL_POST_BRIEFING,
        /// <summary>
        /// Game Level
        /// </summary>
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

    private AudioSource _audioSource;

    private static int _levelIter;

    private static List<string> _sceneList;
    private static List<string> _levelList;
    private static List<LevelInfo> _levelInfoList;


    /// <summary>
    /// The current stats for the loaded level
    /// </summary>
    public static LevelStats CurrentLevelStats { private set; get; }

    /// <summary>
    /// The stats for the previously loaded level
    /// </summary>
    public static LevelStats PreviousLevelStats { private set; get; }

    private void Awake()
    {
        _sceneList = new List<string>();
        _levelList = new List<string>();
        _levelInfoList = new List<LevelInfo>();
        _audioSource = GetComponent<AudioSource>();

        CurrentLevelStats = new LevelStats();
        PreviousLevelStats = new LevelStats();

        var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

        //Grabbing all the scenes
        for (int i = 0; i < sceneCount; i++)
        {
            var str = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
            _sceneList.Add(str);

            //If the scene name starts or contains LEVEL
            //We know it's a level, add it to our LevelList
            if (str.Contains("LEVEL"))
            {
                _levelList.Add(str);
                Debug.Log("Added " + str + " to LevelList");
            }

            Debug.Log("Added " + str + " to SceneList in LevelManager");
        }

        foreach (var info in levelInfoList.GetLevelInfoList())
        {
            _levelInfoList.Add(info);
        }

        Debug.Log("LevelInfoList currently contains " + _levelInfoList.Count + " element(s)");


        if (!lowTimeSound)
        {
            Debug.LogWarning("No Low Time Sound Provided for LevelManager!");
        }

        _audioSource.clip = lowTimeSound;

        Debug.Log("The SceneList in LevelManager contains " + _sceneList.Count.ToString() + " scene(s)");
        Debug.Log("The LevelList in LevelManager contains " + _levelList.Count.ToString() + " level(s)");

        if (FindObjectOfType<LevelPortal>() != null)
        {
            FindObjectOfType<LevelPortal>().OnPlayerEnterPortalEvent += LoadPostBriefing;
            Debug.Log("LevelManager subscribed to OnPlayerEnterPortal");
        }

        if (FindObjectOfType<LevelTimerBox>() != null)
        {
            LevelTimerBox.OnLevelTimerLow += PlayTimeAlarm;
            Debug.Log("LevelManager subscribed to OnLevelTimerLow");
        }

        if (FindObjectOfType<GameManager>() != null)
        {
            GameManager.OnGameStatePlayEvent += LoadNextLevel;
            Debug.Log("LevelManager subscribed to OnGameStatePlay");
        }

        _levelIter = 0;

        PlayLevelButton.OnPlayLevel += PlayLevel;
        PlayLevelButton.OnLevelNumProvided += CheckLevel;
    }

    /// <summary>
    /// I know that this is deprecated but
    /// sceneLoaded event would seemingly break and stop calling after 2-3 new scene loads
    /// Sometimes it would call
    /// Other times it wouldn't (no race condition that I saw, very odd behavior)
    /// This actually calls every time without fail on a new scene load
    /// </summary>
    private void OnLevelWasLoaded(int level)
    {
        if (_audioSource == null)
        {
            Debug.Log("AudioSource is null!");
            _audioSource = GetComponent<AudioSource>();
        }


        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        if (_levelInfoList[gmData.levelNum] != null)
        {
            levelInfo = _levelInfoList[gmData.levelNum];
            Debug.Log("Current Level Name is " + levelInfo.GetLevelName.ToString());
        }

        if (!levelInfo)
        {
            Debug.LogError("No LevelInfo assigned!");
        }

        CurrentLevelStats.CurrentLevelNum = levelInfo.GetLevelNum;
        CurrentLevelStats.CurrentLevelTime = levelInfo.GetLevelTime;
        Debug.Log("Current Level Number is " + CurrentLevelStats.CurrentLevelNum.ToString());

        if (GameDataSerializer._gameDataList.Count > 0)
        {
            var lvlEnd = GameDataSerializer._gameDataList.Count - 1;
            var gmDataNew = GameDataSerializer.LoadGameData(lvlEnd);
            _levelIter = gmDataNew.levelNum;
        }
        else
        {
            _levelIter = 0;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("LevelIter is currently " + _levelIter.ToString());
    }

    private void OnDisable()
    {
        if (FindObjectOfType<LevelPortal>() != null)
        {
            FindObjectOfType<LevelPortal>().OnPlayerEnterPortalEvent -= LoadPostBriefing;
        }

        LevelTimerBox.OnLevelTimerLow -= PlayTimeAlarm;
        GameManager.OnGameStatePlayEvent -= LoadNextLevel;
        PlayLevelButton.OnPlayLevel -= PlayLevel;
        PlayLevelButton.OnLevelNumProvided -= CheckLevel;
    }

    /// <summary>
    /// Loads a Level
    /// </summary>
    /// <param name="levelType">The type of Level to load</param>
    public static void LoadLevel(LEVEL_TYPE levelType)
    {
        switch (levelType)
        {
            case LEVEL_TYPE.LEVEL_NONE:
                Debug.LogError("No level passed in for LoadLevel");
                break;

            case LEVEL_TYPE.LEVEL_MENU:
                SceneManager.LoadScene("MAIN_MENU_2D");
                break;

            case LEVEL_TYPE.LEVEL_PLAY:
                SceneManager.LoadScene(_levelList[_levelIter].ToString());
                break;

            case LEVEL_TYPE.LEVEL_POST_BRIEFING:
                SceneManager.LoadScene("POST_BRIEFING");
                break;

            case LEVEL_TYPE.LEVEL_PRE_BRIEFING:
                SceneManager.LoadScene("PRE_BRIEFING");
                break;

            default:
                break;
        }
    }

    public static void LoadLevel(int levelNum)
    {
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        if (levelNum > 0)
        {
            // Again, this is done because we use the Level Num as offset to the LevelList
            // So if they enter a value > 0
            // We just decrement it by 1
            // User enters 1 in the Input Box
            // We access elem 0 in the LevelList
            // Simple, no?

            levelNum--;
            gmData.levelNum = levelNum;

            if (_levelList[levelNum] != null)
            {
                OnValidLevelFound?.Invoke();
                LoadLevel(LEVEL_TYPE.LEVEL_PRE_BRIEFING);
            }
            else
            {
                Debug.LogError("No level found!");
            }
        }
        else
        {
            Debug.LogError("Invalid number entered!");
        }
    }

    private void CheckForLevel(int levelNum)
    {
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        if (levelNum > 0)
        {
            // Again, this is done because we use the Level Num as offset to the LevelList
            // So if they enter a value > 0
            // We just decrement it by 1
            // User enters 1 in the Input Box
            // We access elem 0 in the LevelList
            // Simple, no?

            levelNum--;
            gmData.levelNum = levelNum;

            if (_levelList[levelNum] != null)
            {
                OnValidLevelFound?.Invoke();
            }
            else
            {
                Debug.LogError("No level found!");
                OnNoValidLevelFound?.Invoke();
            }
        }
        else
        {
            Debug.LogError("Invalid number entered!");
            OnNoValidLevelFound?.Invoke();
        }
    }

    private void LoadNextLevel()
    {
        LoadLevel(LEVEL_TYPE.LEVEL_PLAY);
    }

    private void PlayLevel()
    {
        Debug.Log("Play Level called!");
        LoadLevel(PlayLevelButton.GetNum());
    }

    private void CheckLevel()
    {
        CheckForLevel(PlayLevelButton.GetNum());
    }

    /// <summary>
    /// Returns the current Level Info
    /// </summary>
    /// <returns>The current Level Info</returns>
    public static LevelInfo GetLevelInfo()
    {
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        if (_levelIter != gmData.levelNum)
        {
            _levelIter = gmData.levelNum;
        }

        return _levelInfoList[_levelIter];
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
    
    /// <summary>
    /// An audio cue which is played when the remaining Level time is <= 10
    /// </summary>
    private void PlayTimeAlarm()
    {
        _audioSource.clip = lowTimeSound;
        _audioSource.Play();
    }

   
}

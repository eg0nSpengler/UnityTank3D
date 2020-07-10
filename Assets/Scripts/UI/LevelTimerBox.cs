using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimerBox : MonoBehaviour
{
    public static int FinalLevelTime { get; private set; }

    private TextMeshProUGUI _levelTimeText;
    private float _levelTime;

    public delegate void LevelTimerLow();
    public delegate void LevelTimerEnd();
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

        if (!_levelTimeText)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _levelTimeText = gameObject.AddComponent<TextMeshProUGUI>();
        }

        LevelPortal.OnPlayerEnterPortalEvent += SaveTime;
        PickupDisplayBox.OnPickupScoreEnd += TimeScore;
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
        }
        StartCoroutine(CheckLevelTime());
    }

    private void OnDisable()
    {
        LevelPortal.OnPlayerEnterPortalEvent -= SaveTime;
        PickupDisplayBox.OnPickupScoreEnd -= TimeScore;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    IEnumerator CheckLevelTime()
    {
        yield return new WaitUntil(() => _levelTime < 11);
        InvokeRepeating("InvokeTimerEvent", 0.1f, 1f);
        _levelTimeText.color = Color.red;
        yield return new WaitUntil(() => _levelTime <= 0);
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
        else if (GameManager.GameState == GameManager.GAME_STATE.STATE_POSTBRIEFING)
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

    void TimeScore()
    {
        StartCoroutine(HandleTimeScore());
    }

    IEnumerator HandleTimeScore()
    {
        yield return new WaitForSeconds(0.5f);
        var timeMultipler = 100;
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        while (FinalLevelTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            FinalLevelTime -= 1;
            gmData.playerScore += timeMultipler;
            OnLevelTimerScoreUpdate?.Invoke();
        }

        OnLevelTimerScoreEnd?.Invoke();
    }
}

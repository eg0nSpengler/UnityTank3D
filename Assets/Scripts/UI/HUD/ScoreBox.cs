    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreBox : MonoBehaviour
{
    private TextMeshProUGUI _score;

    private void Awake()
    {
        _score = GetComponent<TextMeshProUGUI>();

        _score.color = Color.green;


    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
    }

    private void OnEnable()
    {
        GameManager.OnGameStatePostBrief += UpdateScoreText;
        LevelTimerBox.OnLevelTimerScoreEnd += UpdateScoreText;
    }

    private void OnDisable()
    {
        GameManager.OnGameStatePostBrief -= UpdateScoreText;
        LevelTimerBox.OnLevelTimerScoreEnd -= UpdateScoreText;
    }

    // Update is called once per frame
    void Update()
    {
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);
        _score.text = gmData.playerScore.ToString();
    }

    void UpdateScoreText()
    {
        Debug.Log("UpdateScoreText called at " + Time.time.ToString());
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        _score.text = gmData.playerScore.ToString();
        
    }
}

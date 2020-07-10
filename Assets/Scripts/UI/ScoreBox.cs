    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBox : MonoBehaviour
{
    private TextMeshProUGUI _score;

    private void Awake()
    {
        _score = GetComponent<TextMeshProUGUI>();

        if (!_score)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _score = gameObject.AddComponent<TextMeshProUGUI>();
        }

        _score.color = Color.green;


        GameManager.OnGameStatePostBrief += UpdateScoreText;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
    }

    private void OnDisable()
    {
        GameManager.OnGameStatePostBrief -= UpdateScoreText;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameState == GameManager.GAME_STATE.STATE_POSTBRIEFING)
        {
            var end = GameDataSerializer._gameDataList.Count - 1;
            var gmData = GameDataSerializer.LoadGameData(end);

            _score.text = gmData.playerScore.ToString();
        }
    }

    void UpdateScoreText()
    {
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        _score.text = gmData.playerScore.ToString();
        
    }
}

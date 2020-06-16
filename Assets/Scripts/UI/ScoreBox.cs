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
        _score.text = "0";
        UpdateScoreText();
        GameManager.OnGameStatePostBrief += UpdateScoreText;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScoreText()
    {
        var gmData = GameDataSerializer.LoadGameData(LevelManager.GetLevelNum());

        if (gmData != null)
        {
            _score.text = gmData.playerScore.ToString();
        }
    }
}

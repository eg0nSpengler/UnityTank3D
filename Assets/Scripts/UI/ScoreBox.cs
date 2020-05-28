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
        PickupManager.OnScoreUpdated += UpdateScoreText;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnDisable()
    {
        PickupManager.OnScoreUpdated -= UpdateScoreText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScoreText()
    {
        _score.text = PickupManager.GetScore().ToString();
    }
}

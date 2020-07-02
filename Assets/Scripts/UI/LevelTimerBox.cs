﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimerBox : MonoBehaviour
{
    private TextMeshProUGUI _levelTime;

    private void Awake()
    {
        _levelTime = GetComponent<TextMeshProUGUI>();
        _levelTime.color = Color.green;

        if (!_levelTime)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _levelTime = gameObject.AddComponent<TextMeshProUGUI>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _levelTime.text = LevelManager.CurrentLevelStats.CurrentLevelTime.ToString();
    }

}

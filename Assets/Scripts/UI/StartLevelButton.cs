﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartLevelButton : MonoBehaviour
{
    public delegate void StartLevel();

    /// <summary>
    /// Called when the user clicks on the Start Level button
    /// </summary>
    public static event StartLevel OnLevelStartEvent;

    private Button _startButton;

    private void Awake()
    {
        _startButton = GetComponent<Button>();

        if (!_startButton)
        {
            Debug.LogError("Failed to get Button on StartLevelButton, creating one now...");
            _startButton = gameObject.AddComponent<Button>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _startButton.onClick.AddListener(HandleMouse);
    }


    private void HandleMouse()
    {
        OnLevelStartEvent?.Invoke();
    }

   
}

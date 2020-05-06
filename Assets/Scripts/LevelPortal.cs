﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Awake()
    {
        gameObject.SetActive(false);
        _audioSource = GetComponent<AudioSource>();

        if (!_audioSource)
        {
            Debug.LogError("No Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.volume = 0.2f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       Debug.Log("At this point, the player would be loaded into the POST-BRIEFING SCENE");
       GameManager.InvokeOnPlayerEnterPortalDelegate();
    }

    private void OnEnable()
    {
        Debug.Log("GATE DETECTED!");
        _audioSource.Play();
    }
}

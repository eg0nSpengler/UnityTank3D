﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider))]
public class LevelPortal : MonoBehaviour
{

    public delegate void PlayerEnteredPortal();
    public delegate void LevelPortalEnabled();

    /// <summary>
    /// Called when the player enters the Level Portal
    /// </summary>
    public event PlayerEnteredPortal OnPlayerEnterPortalEvent;

    /// <summary>
    /// Called when the Level Portal becomes enabled
    /// </summary>
    public event LevelPortalEnabled OnLevelPortalEnabled;

    /// <summary>
    /// The position of the LevelPortal in the Scene
    /// </summary>
    public static Vector3 PortalPos { private set; get; }

    private AudioSource _audioSource;
    private SpriteRenderer _spriteRen;
    private CapsuleCollider _capCol;
    private Animator _anim;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRen = GetComponent<SpriteRenderer>();
        _capCol = GetComponent<CapsuleCollider>();
        _anim = GetComponent<Animator>();

        if (!_anim)
        {
            Debug.LogError("Failed to get Animator on LevelPortal!");
        }

        _audioSource.volume = 0.2f;
        PortalPos = gameObject.transform.position;

        _spriteRen.enabled = false;
        _capCol.enabled = false;
        _capCol.isTrigger = true;

    }

    private void OnEnable()
    {
        Debug.LogWarning(gameObject.name.ToString() + " has been enabled at " + Time.time.ToString());
        PickupManager.OnAllPickupsCollectedEvent += ShowPortal;
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.OnGamePause += StopAnim;
        GameManager.OnGameResume += ResumeAnim;

    }

    private void OnDisable()
    {
        PickupManager.OnAllPickupsCollectedEvent -= ShowPortal;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameManager.OnGamePause -= StopAnim;
        GameManager.OnGameResume -= ResumeAnim;
        Debug.LogWarning(gameObject.name.ToString() + " has been destroyed at " + Time.time.ToString());
    }

    private void OnSceneLoaded(Scene currScene, LoadSceneMode mode)
    {
        Debug.Log(currScene.name.ToString());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == TagStatics.GetPlayerName())
        {
            OnPlayerEnterPortalEvent?.Invoke();
        }
       
    }

    private void ShowPortal()
    {
        _spriteRen.enabled = true;
        _capCol.enabled = true;
        _audioSource.Play();
        OnLevelPortalEnabled?.Invoke();
    }

    void StopAnim()
    {
        _anim.StartPlayback();
    }

    void ResumeAnim()
    {
        _anim.StopPlayback();
    }
}

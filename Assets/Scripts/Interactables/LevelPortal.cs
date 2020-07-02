using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private AudioSource _audioSource;
    private Vector3 _portalPos;

    public delegate void PlayerEnteredPortal();
    public delegate void LevelPortalEnabled();

    /// <summary>
    /// Called when the player enters the Level Portal
    /// </summary>
    public static event PlayerEnteredPortal OnPlayerEnterPortalEvent;

    /// <summary>
    /// Called when the Level Portal becomes enabled
    /// </summary>
    public static event LevelPortalEnabled OnLevelPortalEnabled;

    private void Awake()
    {

        _audioSource = GetComponent<AudioSource>();

        if (!_audioSource)
        {
            Debug.LogError("No Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.volume = 0.2f;
        _portalPos = gameObject.transform.position;
    }

    private void OnEnable()
    {
        OnLevelPortalEnabled();
    }

    private void OnDisable()
    { 
        PickupManager.OnAllPickupsCollectedEvent -= Spawn;
        Debug.LogWarning(gameObject.name.ToString() + " has been destroyed");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        PickupManager.OnAllPickupsCollectedEvent += Spawn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TankActor")
        {
            OnPlayerEnterPortalEvent();
        }
       
    }

    private void Spawn()
    {
        Debug.Log("GATE DETECTED!");
        gameObject.SetActive(true);
        _audioSource.Play();
    }

    /// <summary>
    /// Returns the position of the Level Portal
    /// </summary>
    /// <returns>The Level Portal position, in Vector3 format</returns>
    public Vector3 GetLevelPortalPosition()
    {
        return _portalPos;
    }
}

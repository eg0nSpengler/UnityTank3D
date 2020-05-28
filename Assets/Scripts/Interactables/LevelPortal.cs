using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPortal : MonoBehaviour
{
    private AudioSource _audioSource;

    public delegate void PlayerEnteredPortal();

    /// <summary>
    /// Called when the player enters the Level Portal
    /// </summary>
    public static event PlayerEnteredPortal OnPlayerEnterPortalEvent;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (!_audioSource)
        {
            Debug.LogError("No Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _audioSource.volume = 0.2f;

        
        gameObject.SetActive(false);
        PickupManager.OnAllPickupsCollectedEvent += Spawn;

    }

    private void OnEnable()
    {
        Debug.Log(gameObject.name.ToString() + " has been created");
    }

    private void OnDisable()
    { 
        PickupManager.OnAllPickupsCollectedEvent -= Spawn;
        Debug.LogWarning(gameObject.name.ToString() + " has been destroyed");
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
}

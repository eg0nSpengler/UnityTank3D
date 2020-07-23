using System.Collections;
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

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _spriteRen = GetComponent<SpriteRenderer>();
        _capCol = GetComponent<CapsuleCollider>();

        if (!_audioSource)
        {
            Debug.LogError("No Audio Source on " + gameObject.name.ToString() + ", creating one now");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!_spriteRen)
        {
            Debug.LogError("Failed to get Sprite Renderer on LevelPortal, creating one now");
            _spriteRen = gameObject.AddComponent<SpriteRenderer>();
        }

        if (!_capCol)
        {
            Debug.LogError("Failed to get CapsuleCollider on LevelPortal, creating one now");
            _capCol = gameObject.AddComponent<CapsuleCollider>();
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
    }

    private void OnDisable()
    {
        PickupManager.OnAllPickupsCollectedEvent -= ShowPortal;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.LogWarning(gameObject.name.ToString() + " has been destroyed at " + Time.time.ToString());
    }

    // Start is called before the first frame update
    void Start()
    {

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

}

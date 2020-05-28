using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : MonoBehaviour
{

    [Header("References")]
    public GameObject projectile;
    public Transform gunTransform;

    public delegate void GunStatusUpdated();

    /// <summary>
    /// Called when the gun status is updated
    /// </summary>
    public static event GunStatusUpdated OnGunStatusUpdate;

    private AudioSource _audioSource;

    private static float _reloadTime;
    private static GUN_STATUS gunStatus;
    private float _timeSinceLastShot;

    private enum GUN_STATUS
    {
        NONE,
        GUN_READY,
        REARMING,
        CHARGING
    }
    
    private void Awake()
    {
        gunTransform = GetComponent<Transform>();
        _audioSource = GetComponentInParent<AudioSource>();
        

        if (!gunTransform)
        {
            Debug.LogError("No gun transform set in TankGun instance on " + gameObject.name.ToString() + ", setting gun transform to parent transform");
            gunTransform = gameObject.transform;
        }

        if (!projectile)
        {
            Debug.LogError("No projectile found on " + gameObject.name.ToString() + ", you probably forgot to set it in the Inspector!");
        }

        if (!_audioSource)
        {
            Debug.LogError("No Audio Source found on " + gameObject.name.ToString() + ", creating one now..");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _reloadTime = _audioSource.clip.length;
        _timeSinceLastShot = _reloadTime;
        gunStatus = GUN_STATUS.GUN_READY; 

    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        _timeSinceLastShot += Time.deltaTime;

        if (IsGunReady() == true)
        {
            UpdateGunStatus(GUN_STATUS.GUN_READY);
        }
        else
        {
            UpdateGunStatus(GUN_STATUS.REARMING);
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireGun();
        }
    }

    private bool IsGunReady()
    {
        if (_timeSinceLastShot >= _reloadTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void FireGun()
    {
        if (IsGunReady())
        {
            _audioSource.Play();
            Instantiate(projectile, gunTransform.position, gunTransform.rotation);
            _timeSinceLastShot = 0.0f;
        }
        else
        {
            Debug.LogWarning("Gun not ready to fire on TankGun!");
        }
    }
    
    void UpdateGunStatus(GUN_STATUS newStatus)
    {
        switch (newStatus)
        {
            case GUN_STATUS.GUN_READY:
                gunStatus = GUN_STATUS.GUN_READY;
                break;

            case GUN_STATUS.REARMING:
                gunStatus = GUN_STATUS.REARMING;
                break;

            case GUN_STATUS.CHARGING:
                gunStatus = GUN_STATUS.CHARGING;
                break;

            case GUN_STATUS.NONE:
                gunStatus = GUN_STATUS.NONE;
                break;

            default:
                break;
        }

        
       OnGunStatusUpdate();

    }

    /// <summary>
    /// Returns the current Gun Status in string format
    /// </summary>
    /// <returns></returns>
    public static string GunStatusToString()
    {
        switch(gunStatus)
        {
            case GUN_STATUS.GUN_READY:
                return "GUN READY";

            case GUN_STATUS.REARMING:
                return "REARMING";

            case GUN_STATUS.CHARGING:
                return "RECHARGING";

            case GUN_STATUS.NONE:
                Debug.LogError("GunStatus in TankGun is currently NONE");
                Debug.LogError("Double check the firing/reload timers");
                return "NONE";
        }
        return "NONE";
    }
}

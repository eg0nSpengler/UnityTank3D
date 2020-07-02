using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : MonoBehaviour
{

    [Header("References")]
    public GameObject projectile;
    public Transform gunTransform;

    [Header("Audio References")]
    public AudioClip fireSound;
    public AudioClip rearmSound; // An audio cue which is played when the gun has finished reloading
    public AudioClip maxChargeSound; // A audio cue which is played when the gun reaches maximum charge
    

    public delegate void GunStatusUpdated();

    /// <summary>
    /// Called when the gun status is updated
    /// </summary>
    public static event GunStatusUpdated OnGunStatusUpdate;

    private AudioSource _audioSource;

    private int  _reloadTime;
    private float _gunCharge;
    private float _maxChargeTime; 
    private bool isReadyToFire; // Is the Tank Gun ready to fire?
    private bool isChargeCuePlayed; // Has the MAX CHARGE audio cue been played?
    private GUN_STATUS gunStatus;

    private enum GUN_STATUS
    {
        NONE,
        GUN_READY,
        REARMING,
        CHARGING,
        MAX_CHARGE
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

        if (!fireSound)
        {
            Debug.LogWarning("No Fire Sound provided for TankGun!");
        }

        if (!rearmSound)
        {
            Debug.LogWarning("No Rearm Sound provided for TankGun!");
        }

        if (!maxChargeSound)
        {
            Debug.LogWarning("No Max Charge sound provided for TankGun!");
        }

        _reloadTime = 1;
        _maxChargeTime = 2.0f;
        _gunCharge = 0.0f;
        isReadyToFire = true;
        isChargeCuePlayed = false;
        gunStatus = GUN_STATUS.GUN_READY;
        

    }

    void Start()
    {

    }

    void OnDisable()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKey(KeyCode.LeftControl) == true)
        {
            ChargeGun();
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) == true)
        {
            FireGun();
        }
    }

    
    /// <summary>
    /// Charges the Tank Gun while LeftCtrl is held down
    /// Reaching MAX charge makes the projectile pass through multiple monsters
    /// </summary>
    void ChargeGun()
    {
        if (isReadyToFire == true) // We check to see if the gun is ready to fire before we begin charging
        {
            _gunCharge += Time.deltaTime;
            UpdateGunStatus(GUN_STATUS.CHARGING);

            if (_gunCharge >= _maxChargeTime) // Are we at MAX weapon charge?
            {
                _gunCharge = _maxChargeTime;
                UpdateGunStatus(GUN_STATUS.MAX_CHARGE);

                if (isChargeCuePlayed == false) // Let's play the Audio Cue to notify the player we've reached MAX charge
                {
                    isChargeCuePlayed = true;
                    _audioSource.PlayOneShot(maxChargeSound);
                }
            }
        }
    }

    void FireGun()
    {
        if (isReadyToFire == true)
        {
            _audioSource.PlayOneShot(fireSound);
            _gunCharge = 0.0f;
            isChargeCuePlayed = false;
            Instantiate(projectile, gunTransform.position, gunTransform.rotation);
            UpdateGunStatus(GUN_STATUS.REARMING);
            StartCoroutine(ReloadGun());
        }
    }

    /// <summary>
    /// A Coroutine that handles the "reloading" of the Tank Gun
    /// Nothing too complex, we just wait for X seconds before setting the Tank Gun to READY
    /// </summary>
    IEnumerator ReloadGun()
    {
        
        isReadyToFire = false;
        yield return new WaitForSeconds(_reloadTime);
        UpdateGunStatus(GUN_STATUS.GUN_READY);
        isReadyToFire = true;
        _audioSource.PlayOneShot(rearmSound);
    }

    /// <summary>
    /// Updates the Gun Status
    /// </summary>
    /// <param name="newStatus">The new status for the Tank Gun</param>
    void UpdateGunStatus(GUN_STATUS newStatus)
    {
        switch(newStatus)
        {
            case GUN_STATUS.GUN_READY:
                gunStatus = GUN_STATUS.GUN_READY;
                break;

            case GUN_STATUS.CHARGING:
                gunStatus = GUN_STATUS.CHARGING;
                break;

            case GUN_STATUS.MAX_CHARGE:
                gunStatus = GUN_STATUS.MAX_CHARGE;
                break;

            case GUN_STATUS.REARMING:
                gunStatus = GUN_STATUS.REARMING;
                break;

            case GUN_STATUS.NONE:
                gunStatus = GUN_STATUS.NONE;
                Debug.LogError("TankGun status is currently NONE");
                Debug.LogError("Check the reload timer");
                break;

            default:
                break;
        }

        OnGunStatusUpdate();
    }

    /// <summary>
    /// Returns the current Gun Status in string format
    /// </summary>
    /// <returns>The current TankGun status, in string format</returns>
    public string GunStatusToString()
    {
        switch(gunStatus)
        {
            case GUN_STATUS.GUN_READY:
                return "GUN READY";

            case GUN_STATUS.REARMING:
                return "REARMING";

            case GUN_STATUS.CHARGING:
                return "CHARGING";

            case GUN_STATUS.MAX_CHARGE:
                return "MAX CHARGE";

            case GUN_STATUS.NONE:
                Debug.LogError("GunStatus in TankGun is currently NONE");
                Debug.LogError("Double check the firing/reload timers");
                return "NONE";
        }
        return "NONE";
    }
}

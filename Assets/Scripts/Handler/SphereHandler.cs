using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHandler : MonoBehaviour
{
    [Header("Audio References")]
    public AudioClip playerPickupSound;
    public AudioClip monsterPickupSound;
    
    /// <summary>
    /// Has this pickup been collected?
    /// </summary>
    public bool IsCollected { protected set; get; }

    public delegate void PickupCollected();
    public delegate void PickupDie();

    /// <summary>
    /// Called when a pickup is collected
    /// </summary>
    public event PickupCollected OnPickupCollectedEvent;

    /// <summary>
    /// Called when a pickup "dies", primarily used for civilian pickups.
    /// </summary>
    public event PickupDie OnPickupDieEvent;


    private CivAnimationHandler _animHandler;
    private CapsuleCollider _capsuleCollider;

    //A boolean that we set depending on who collects the pickup
    //TRUE if the player
    //FALSE if a monster
    private bool _pickupCollector;

    private void Awake()
    {
        _animHandler = GetComponent<CivAnimationHandler>();
        _capsuleCollider = GetComponent<CapsuleCollider>();

        _pickupCollector = false;
        IsCollected = false;

        if (!playerPickupSound)
        {
            Debug.LogWarning("No Player Pickup sound provided for " + gameObject.name.ToString());
        }

        if (!monsterPickupSound)
        {
            Debug.LogWarning("No Monster Pickup sound provided for " + gameObject.name.ToString());
        }

        if (!_animHandler)
        {
            Debug.LogError("Failed to get CivAnimationHandler on " + gameObject.name.ToString() + " creating one now");
            _animHandler = gameObject.AddComponent<CivAnimationHandler>();
        }


        if (!_capsuleCollider)
        {
            Debug.LogError("Failed to get CapsuleCollider on " + gameObject.name.ToString() + " creating one now");
            _capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        }

        _animHandler.OnDeathAnimPlay += HandleDeathAnim;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        _animHandler.OnDeathAnimPlay -= HandleDeathAnim;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == TagStatics.GetPlayerName())
        {
            _pickupCollector = true;
            IsCollected = true;
            OnSphereDestroyed(_pickupCollector);
        }

        if (other.gameObject.name.Contains("Monster"))
        {
            _pickupCollector = false;
            IsCollected = true;
            OnSphereDestroyed(_pickupCollector);
        }

        if (other.gameObject.name.Contains("Projectile"))
        {
            _pickupCollector = false;
            IsCollected = true;
            OnPickupDieEvent();
            OnSphereDestroyed(_pickupCollector);
        }

    }

    private void OnSphereDestroyed(bool collector)
    {
            if (collector == true)
            {
                AudioSource.PlayClipAtPoint(playerPickupSound, gameObject.transform.position);
                gameObject.SetActive(false);
            }
            else
            {
                AudioSource.PlayClipAtPoint(monsterPickupSound, gameObject.transform.position);
            }

            OnPickupCollectedEvent();

    }

    /// <summary>
    /// Returns a boolean value depending on who collects the Pickup
    /// TRUE if the player
    /// FALSE if a monster
    /// </summary>
    /// <returns></returns>
    public bool GetPickupCollector()
    {
        return _pickupCollector;
    }

    private void HandleDeathAnim()
    {
        StartCoroutine(DeathAnimCoroutine());
    }

    IEnumerator DeathAnimCoroutine()
    {
        // This is done so that the civilian falls to the floor 
        // And doesn't just float in mid-air
        _capsuleCollider.isTrigger = false;
        _capsuleCollider.height = 0.20f;
        _capsuleCollider.direction = Vector3Int.right.x;

        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}

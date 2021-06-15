using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CivAnimationHandler))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class SphereHandler : MonoBehaviour
{
    [Header("Audio References")]
    public AudioClip playerPickupSound;
    public AudioClip monsterPickupSound;
    

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

    /// <summary>
    /// Has this pickup been collected?
    /// </summary>
    public bool IsCollected { set; get; }

    /// <summary>
    /// A boolean that we set depending on who collects the pickup
    /// <para>TRUE if the player</para>
    /// <para>FALSE if a monster (Or if the player shot the pickup)</para>
    /// </summary>
    public bool PickupCollector {set; get; }

    private CivAnimationHandler _animHandler;
    private CapsuleCollider _capsuleCollider;
    private Rigidbody _rb;


    private void Awake()
    {
        _animHandler = GetComponent<CivAnimationHandler>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();

        IsCollected = false;
        PickupCollector = false;

        if (!playerPickupSound)
        {
            Debug.LogWarning("No Player Pickup sound provided for " + gameObject.name.ToString());
        }

        if (!monsterPickupSound)
        {
            Debug.LogWarning("No Monster Pickup sound provided for " + gameObject.name.ToString());
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnEnable()
    {
        _animHandler.OnDeathAnimPlay += HandleDeathAnim;
    }
    private void OnDisable()
    {
        _animHandler.OnDeathAnimPlay -= HandleDeathAnim;
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsCollected == false)
        {
            if (other.gameObject.name == TagStatics.GetPlayerName())
            {
                PickupCollector = true;
                IsCollected = true;
                OnSphereDestroyed(PickupCollector);
            }

            if (other.gameObject.name.Contains("Monster"))
            {
                PickupCollector = false;
                IsCollected = true;
                OnPickupDieEvent?.Invoke();
                OnSphereDestroyed(PickupCollector);
            }

            if (other.gameObject.name.Contains("Projectile"))
            {
                PickupCollector = false;
                IsCollected = true;
                OnPickupDieEvent?.Invoke();
                OnSphereDestroyed(PickupCollector);
            }
            
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

            OnPickupCollectedEvent?.Invoke();

    }

    private void HandleDeathAnim()
    {
        StartCoroutine(DeathAnimCoroutine());
    }

    IEnumerator DeathAnimCoroutine()
    {
        // This is done so that the civilian falls to the floor and doesn't just float in mid air
        _capsuleCollider.isTrigger = false;
        _capsuleCollider.height = 0.20f;
        _capsuleCollider.center = new Vector3(_capsuleCollider.center.x, 0.03f, _capsuleCollider.center.y);
        _capsuleCollider.direction = Vector3Int.right.x;

        yield return new WaitForSeconds(1f);

        // So we can walk through corpses
        _capsuleCollider.isTrigger = true;
        _rb.useGravity = false;
        _rb.freezeRotation = true;
    }
}

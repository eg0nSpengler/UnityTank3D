using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHandler : MonoBehaviour
{
    [Header("References")]
    public AudioClip playerPickupSound;
    public AudioClip monsterPickupSound;
    
    //A boolean that we set depending on who collects the pickup
    //TRUE if the player
    //FALSE if a monster
    private bool _pickupCollector;

    public delegate void PickupCollected();

    /// <summary>
    /// Called when a pickup is collected
    /// </summary>
    public static event PickupCollected OnPickupCollectedEvent;

    private void Awake()
    {
        if (!playerPickupSound)
        {
            Debug.LogWarning("No Player Pickup sound provided for " + gameObject.name.ToString());
        }

        if (!monsterPickupSound)
        {
            Debug.LogWarning("No Monster Pickup sound provided for " + gameObject.name.ToString());
        }

        _pickupCollector = false;
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
            _pickupCollector = true;
            OnSphereDestroyed(_pickupCollector);
        }

        if (other.gameObject.name.Contains("Monster"))
        {
            _pickupCollector = false;
            OnSphereDestroyed(_pickupCollector);
        }

    }

    private void OnSphereDestroyed(bool collector)
    {
        if (collector == true)
        {
            AudioSource.PlayClipAtPoint(playerPickupSound, gameObject.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(monsterPickupSound, gameObject.transform.position);
        }

        gameObject.SetActive(false);
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

}

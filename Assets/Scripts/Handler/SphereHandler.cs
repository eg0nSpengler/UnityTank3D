using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereHandler : MonoBehaviour
{
    [Header("References")]
    public AudioClip pickupSound;

    public delegate void PickupCollected();

    /// <summary>
    /// Called when a pickup is collected
    /// </summary>
    public static event PickupCollected OnPickupCollectedEvent;

    private void Awake()
    {
        if (!pickupSound)
        {
            Debug.LogError("No Pickup sound provided for " + gameObject.name.ToString());
        }

        
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
        if (other.gameObject.name == "TankActor" )
        {
            Debug.Log("Collision detected!");
            OnSphereDestroyed();
        }
         
    }

    private void OnSphereDestroyed()
    {
        AudioSource.PlayClipAtPoint(pickupSound, gameObject.transform.position);
        Destroy(gameObject);
        OnPickupCollectedEvent();
    }

}

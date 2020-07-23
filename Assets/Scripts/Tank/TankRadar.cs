using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRadar : MonoBehaviour
{
    public delegate void PickupInRange();

    /// <summary>
    /// Called when a Pickup enters our radius
    /// </summary>
    public static event PickupInRange OnPickupInRange;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        
    }

}

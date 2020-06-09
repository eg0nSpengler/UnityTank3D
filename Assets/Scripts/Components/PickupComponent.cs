using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupComponent : MonoBehaviour
{
    [SerializeField]
    private bool _isCollected;

    /// <summary>
    /// Has this Pickup been collected?
    /// </summary>
    public bool IsCollected
    {
        get 
        {
            return _isCollected;
        }

        set
        {
            _isCollected = value;
        }
    }
    private void Awake()
    {
        if (gameObject.tag != "Pickup")
        {
            Debug.LogWarning(gameObject.name.ToString() + " has a PickupComponent attached to it but is not tagged as a Pickup!");
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
}

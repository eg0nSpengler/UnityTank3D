using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_SimpleMob : MonoBehaviour
{
    private CapsuleCollider _capsuleCollider;

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();

        if(!_capsuleCollider)
        {
            Debug.LogError("Failed to get CapsuleCollider on " + gameObject.name.ToString() + ", creating one now...");
            _capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
        }

    }

    
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
     
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Debug.LogWarning(gameObject.name.ToString() + " has been hit!");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnDisable()
    {
        Debug.LogWarning(gameObject.name.ToString() + " has been disabled!");
    }


}

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
            MobManager.InvokeMobTakeDamage(gameObject, 2);
        }

        Debug.LogWarning(gameObject.name.ToString() + " has had a collision with " + collision.gameObject.name.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            return;
        }

        Debug.LogWarning(gameObject.name.ToString() + " has had a collision with " + other.gameObject.name.ToString());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (!_rb)
        {
            Debug.LogError("ProjectileHandler failed to get Rigidbody, creating one now.");
            _rb = gameObject.AddComponent<Rigidbody>();
        }
        else 
        {
            Debug.Log("Rigidbody found on Projectile!");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Tank Projectile created!");
        
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        _rb.AddForce(gameObject.transform.forward * 100.0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Projectile has collied with " + collision.gameObject.ToString());
        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        Debug.Log(name.ToString() + " has been destroyed!");
    }

}

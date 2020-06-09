using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    [Header("Variables")]
    public int damageAmount = 0;

    private Rigidbody _rb;

    public delegate void DamageMob(GameObject obj, int dmg);

    /// <summary>
    /// Called when a mob is hit by a projectile
    /// </summary>
    public static event DamageMob OnDamageMobEvent;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (!_rb)
        {
            Debug.LogError("ProjectileHandler failed to get Rigidbody, creating one now.");
            _rb = gameObject.AddComponent<Rigidbody>();
        }

    }

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log("Projectile has collied with " + collision.gameObject.name.ToString());
        Destroy(gameObject);
        if (collision.collider.gameObject.tag == "Mob")
        {
            OnDamageMobEvent(collision.collider.gameObject, damageAmount);
        }
    }

    

    private void OnDestroy()
    {
        Debug.Log(name.ToString() + " has been destroyed!");
    }

}

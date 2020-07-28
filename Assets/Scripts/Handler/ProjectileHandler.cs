using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileHandler : MonoBehaviour
{
    [Header("Variables")]
    public int damageAmount;

    private Rigidbody _rb;
    private RaycastHit _hit;
    private Vector3 _impactPoint;
    public delegate void DamageMob(GameObject obj, int dmg);
    public delegate void HitObject();

    private bool IsSleep;

    /// <summary>
    /// Called when a mob is hit by a projectile
    /// </summary>
    public static event DamageMob OnDamageMobEvent;

    /// <summary>
    /// Called when a projectile hits something in the scene, whether it be a wall or a Mob
    /// </summary>
    public static event HitObject OnHitObject;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        damageAmount = 2;

        if (!_rb)
        {
            Debug.LogError("ProjectileHandler failed to get Rigidbody, creating one now.");
            _rb = gameObject.AddComponent<Rigidbody>();
        }
        IsSleep = false;

        GameManager.OnGamePause += SleepSelf;
        GameManager.OnGameResume += WakeSelf;
    }

    private void OnDisable()
    {
        GameManager.OnGamePause -= SleepSelf;
        GameManager.OnGameResume -= WakeSelf;
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_rb.SweepTest(transform.forward, out _hit, 1000f) == true)
        {
            _impactPoint = _hit.point;
        }
    }

    private void FixedUpdate()
    {
        if (IsSleep == false)
        {
            _rb.AddForce(gameObject.transform.forward * 100.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == TagStatics.GetLevelTag())
        {
            OnHitObject?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == TagStatics.GetMobTag())
        {
            if (other.gameObject.name != TagStatics.GetPlayerName())
            {
                OnDamageMobEvent?.Invoke(other.gameObject, damageAmount);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.tag == TagStatics.GetPickupTag())
        {
            OnHitObject?.Invoke();
            Destroy(gameObject);
        }

        if (other.gameObject.tag == TagStatics.GetLevelTag())
        {
            OnHitObject.Invoke();
            Destroy(gameObject);
        }

    }
    private void OnDestroy()
    {
        Debug.Log(name.ToString() + " has been destroyed!");
    }

    void SleepSelf()
    {
        StartCoroutine(SleepSelfRoutine());
    }

    void WakeSelf()
    {
        StartCoroutine(WakeSelfRoutine());
    }

    IEnumerator SleepSelfRoutine()
    {
        IsSleep = true;

        while (IsSleep == true)
        {
            _rb.Sleep();
            yield return null;
        } 
    }

    IEnumerator WakeSelfRoutine()
    {
        IsSleep = false;
        yield return null;
    }
}

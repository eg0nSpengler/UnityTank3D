using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MeleeAttack : MonoBehaviour
{
    [Header("Variables")]
    public int damageAmount;

    public delegate void DamageMob(GameObject obj, int dmg);
    public delegate void AttackMob();
    public delegate void AttackMobComplete();
    

    /// <summary>
    /// Called when a mob is hit by our melee attack
    /// </summary>
    public static event DamageMob OnDamageMobEvent;

    /// <summary>
    /// Called when this mob is attacking
    /// </summary>
    public event AttackMob OnMobAttack;

    /// <summary>
    /// Called when this mob has finished attacking
    /// </summary>
    public event AttackMobComplete OnMobAttackComplete;

    private NavComponent _navComponent;
    private DetectionSphere _detectionSphere;
    private BoxCollider _boxCollider;
    private HealthComponent _healthComp;

    private Vector3 _boxDefaultSize;
    private Vector3 _boxAttackSize;


    private void Awake()
    {
        _navComponent = GetComponent<NavComponent>();
        _detectionSphere = GetComponent<DetectionSphere>();
        _boxCollider = GetComponentInParent<BoxCollider>();
        _healthComp = GetComponentInParent<HealthComponent>();

        damageAmount = 10;

        if(!_navComponent)
        {
            Debug.Log("Failed to get Nav Component " + gameObject.name.ToString() + ", creating one now");
            _navComponent = gameObject.AddComponent<NavComponent>();
        }

        if (!_detectionSphere)
        {
            Debug.Log("Failed to get DetectionSphere on " + gameObject.name.ToString() + ", creating one now");
            _detectionSphere = gameObject.AddComponent<DetectionSphere>();
        }

        if (!_boxCollider)
        {
            Debug.LogError("Failed to get BoxCollider on " + gameObject.name.ToString() + ", creating one now");
            _boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        if (!_healthComp)
        {
            Debug.LogError("Failed to get Health Component on" + gameObject.name.ToString() + " creating one now");
            _healthComp = gameObject.AddComponent<HealthComponent>();
        }

        _boxDefaultSize = _boxCollider.size;
        _boxAttackSize = new Vector3(0.05f, 0.05f, 1f);

        
    }
    private void OnEnable()
    {
        _detectionSphere.OnTargetTracking += AttackTarget;
        _healthComp.OnHealthZero += DisableSelf;
    }

    private void OnDisable()
    {
        _detectionSphere.OnTargetTracking -= AttackTarget;
        _healthComp.OnHealthZero -= DisableSelf;
        StopAllCoroutines();
    }


    void AttackTarget(GameObject obj)
    {
        if (obj.GetComponent<HealthComponent>().IsDead == false)
        {
            OnDamageMobEvent?.Invoke(obj, damageAmount);
            StartCoroutine(AttackTimer());
            OnMobAttack?.Invoke();
        }
    }

    IEnumerator AttackTimer()
    {
        _boxCollider.size = _boxAttackSize;
        yield return new WaitForSeconds(2);
        _boxCollider.size = _boxDefaultSize;
        OnMobAttackComplete?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled)
        {
            if (other.gameObject.tag == TagStatics.GetMobTag() && other.gameObject.name == TagStatics.GetPlayerName())
            {
                AttackTarget(other.gameObject);
            }        
        }
    }

    void DisableSelf()
    {
        // This is done so that we can't somehow can still attack the player even though we're dead.
        this.enabled = false;
    }

}

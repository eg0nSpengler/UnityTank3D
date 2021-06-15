using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(DemonAnimationHandler))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public class AI_SimpleMob : MonoBehaviour
{
    private AudioSource _audioSource;
    private HealthComponent _healthComp;
    private DemonAnimationHandler _animHandler;
    private Rigidbody _rb;
    private BoxCollider _parentCollider;
    private SphereCollider _sphereCollider;
    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        _healthComp = GetComponentInParent<HealthComponent>();
        _animHandler = GetComponentInParent<DemonAnimationHandler>();
        _rb = GetComponentInParent<Rigidbody>();
        _parentCollider = GetComponentInParent<BoxCollider>();
        _sphereCollider = GetComponent<SphereCollider>();

        if (!_audioSource)

        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + " creating one now");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!_healthComp)
        {
            Debug.LogError("Failed to get Health Component on " + gameObject.name.ToString() + " creating one now");
            _healthComp = gameObject.AddComponent<HealthComponent>();
        }

        if (!_animHandler)
        {
            Debug.LogError("Failed to get DemonAnimationHandler on " + gameObject.name.ToString() + " creating one now");
            _animHandler = gameObject.AddComponent<DemonAnimationHandler>();
        }

        if (!_rb)
        {
            Debug.LogError("Failed to get Rigidbody on " + gameObject.name.ToString() + " creating one now");
            _rb = gameObject.AddComponent<Rigidbody>();
        }

        if (!_parentCollider)
        {
            Debug.LogError("Failed to get BoxCollider on " + gameObject.name.ToString() + " creating one now");
            _parentCollider = gameObject.AddComponent<BoxCollider>();
        }

        if (!_sphereCollider)
        {
            Debug.LogError("Failed to get SphereCollider on " + gameObject.name.ToString() + " creating one now");
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
        }

        //_rb.useGravity = true;

    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        _animHandler.OnDeathAnimationPlay += HandleDeathAnim;
        _healthComp.OnHealthZero += DoMonsterDeath;
    }

    private void OnDisable()
    {
        _animHandler.OnDeathAnimationPlay -= HandleDeathAnim;
        _healthComp.OnHealthZero -= DoMonsterDeath;
        StopAllCoroutines();
    }


    /// <summary>
    /// Like in the SphereHandler script
    /// This is done so the corpse falls to the floor
    /// </summary>
    void HandleDeathAnim()
    {
        StartCoroutine(DeathAnimCoroutine());
    }

    IEnumerator DeathAnimCoroutine()
    {
        _parentCollider.size = new Vector3(0.5f, 0.1f, 0.1f);

        yield return new WaitForSeconds(1f);

        //So we can walk through corpses
        _rb.useGravity = false;
        _rb.freezeRotation = true;
        _parentCollider.isTrigger = true;
       
    }

    void DoMonsterDeath()
    {
        _sphereCollider.enabled = false;
        AudioSource.PlayClipAtPoint(_audioSource.clip, gameObject.transform.position);
    }

}

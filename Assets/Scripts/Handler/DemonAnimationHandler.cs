using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for handling the Demon Monster Animation states
/// </summary>
/// 

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavComponent))]
[RequireComponent(typeof(AI_MeleeAttack))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HealthComponent))]
public class DemonAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private NavComponent _navComp;
    private AI_MeleeAttack _attack;
    private Rigidbody _rb;
    private HealthComponent _healthComp;

    //This is to get our IsNavigating param from our Animator
    private int _isNavigatingHash;

    //This is to get our IsAttacking param from our Animator
    private int _isAttackingHash;

    //This is to get our IsDead param from our Animator
    private int _isDeadHash;

    public delegate void WalkAnimationPlay();

    public delegate void DeathAnimationPlay();

    /// <summary>
    /// Called when the walk animation plays
    /// </summary>
    public event WalkAnimationPlay OnWalkAnimationPlay;

    /// <summary>
    /// Called when the death animation plays
    /// </summary>
    public event DeathAnimationPlay OnDeathAnimationPlay;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _navComp = GetComponentInChildren<NavComponent>();
        _attack = GetComponentInChildren<AI_MeleeAttack>();
        _rb = GetComponent<Rigidbody>();
        _healthComp = GetComponent<HealthComponent>();

        _isNavigatingHash = Animator.StringToHash("IsNavigating");
        _isAttackingHash = Animator.StringToHash("IsAttacking");
        _isDeadHash = Animator.StringToHash("IsDead");

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        _navComp.OnDestinationBegin += HandleWalkAnim;
        _navComp.OnDestinationSuccess += ResetTrigger;
        _attack.OnMobAttack += HandleAttackAnim;
        _attack.OnMobAttackComplete += ResetAttackTrigger;
        _healthComp.OnHealthZero += HandleDeathAnim;
    }
    private void OnDisable()
    {
        _navComp.OnDestinationBegin -= HandleWalkAnim;
        _navComp.OnDestinationSuccess -= ResetTrigger;
        _attack.OnMobAttack -= HandleAttackAnim;
        _attack.OnMobAttackComplete -= ResetAttackTrigger;
        _healthComp.OnHealthZero -= HandleDeathAnim;
    }

    void HandleWalkAnim()
    {
        //Sets isNavigating to true, triggering the walk animation
        _animator.SetTrigger(_isNavigatingHash);

        _rb.useGravity = true;
    }

    void HandleAttackAnim()
    {
        _animator.SetTrigger(_isAttackingHash);
    }

    void HandleDeathAnim()
    {
        _animator.SetTrigger(_isDeadHash);

        _rb.useGravity = true;

        OnDeathAnimationPlay?.Invoke();
    }

    void ResetTrigger()
    {
        // This sets the demon back to the idle state (occurs when they aren't attacking or successfully navigated to a location)
        _animator.ResetTrigger(_isNavigatingHash);
    }

    void ResetAttackTrigger()
    {
        _animator.ResetTrigger(_isAttackingHash);
    }
}

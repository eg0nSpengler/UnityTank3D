using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class CivAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private SphereHandler _sphereHandler;
    private Rigidbody _rb;
   

    /// <summary>
    /// An array of animation clips used by our Animator
    /// </summary>
    private AnimationClip[] _animClips;

    //This is to get our IsDead param from our Animator
    private int _isDeadHash;


    public delegate void DeathAnimationPlay();

    /// <summary>
    /// Called when the death animation plays
    /// </summary>
    public event DeathAnimationPlay OnDeathAnimPlay;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sphereHandler = GetComponent<SphereHandler>();
        _rb = GetComponent<Rigidbody>();


        if (!_animator)
        {
            Debug.LogError("Failed to get Animator on " + gameObject.name.ToString() + ", creating one now");
            _animator = gameObject.AddComponent<Animator>();
        }


        if (!_sphereHandler)
        {
            Debug.LogError("Failed to get SphereHandler on " + gameObject.name.ToString() + ", creating one now");
            _sphereHandler = gameObject.AddComponent<SphereHandler>();
        }


        if (!_rb)
        {
            Debug.LogError("Failed to get Rigidbody on " + gameObject.name.ToString() + ", creating one now");
            _rb = gameObject.AddComponent<Rigidbody>();
        }


        _animClips = _animator.runtimeAnimatorController.animationClips;

        _isDeadHash = Animator.StringToHash("IsDead");

        _sphereHandler.OnPickupDieEvent += HandleDeathAnim;
    }

    private void OnDisable()
    {
        _sphereHandler.OnPickupDieEvent -= HandleDeathAnim;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleDeathAnim()
    {
        //We set the IsDead param to true in the Animator, trigging the death animation
        _animator.SetTrigger(_isDeadHash);

         _rb.useGravity = true;

        OnDeathAnimPlay();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SphereHandler))]
[RequireComponent(typeof(Rigidbody))]
public class CivAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private SphereHandler _sphereHandler;
    private Rigidbody _rb;


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

        _isDeadHash = Animator.StringToHash("IsDead");

        GameManager.OnGamePause += StopAnim;
        GameManager.OnGameResume += ResumeAnim;
    }

    private void OnEnable()
    {
        _sphereHandler.OnPickupDieEvent += HandleDeathAnim;
    }

    private void OnDisable()
    {
        _sphereHandler.OnPickupDieEvent -= HandleDeathAnim;
        GameManager.OnGamePause -= StopAnim;
        GameManager.OnGameResume -= ResumeAnim;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void HandleDeathAnim()
    {
        //We set the IsDead param to true in the Animator, triggering the death animation
        _animator.SetTrigger(_isDeadHash);

         _rb.useGravity = true;

        OnDeathAnimPlay?.Invoke();
    }

    void StopAnim()
    {
        _animator.StartPlayback();
    }

    void ResumeAnim()
    {
        _animator.StopPlayback();
    }
}

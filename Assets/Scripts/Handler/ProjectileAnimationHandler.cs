using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ProjectileAnimationHandler : MonoBehaviour
{

    private Animator _animator;

    /// <summary>
    /// This is to get our HasHit param from our Animator
    /// </summary>
    private int _hasHitHash;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        if (!_animator)
        {
            Debug.LogError("Failed to get Animator on ProjectileAnimationHandler" + " creating one now");
            _animator = gameObject.AddComponent<Animator>();
        }

        _hasHitHash = Animator.StringToHash("HasHit");

    }

    private void OnEnable()
    {
        ProjectileHandler.OnHitObject += HandleHitAnim;
        GameManager.OnGamePause += StopAnim;
        GameManager.OnGameResume += ResumeAnim;
    }

    private void OnDisable()
    {

        ProjectileHandler.OnHitObject -= HandleHitAnim;
        GameManager.OnGamePause -= StopAnim;
        GameManager.OnGameResume -= ResumeAnim;

    }

    void HandleHitAnim()
    {
        _animator.SetTrigger(_hasHitHash);
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

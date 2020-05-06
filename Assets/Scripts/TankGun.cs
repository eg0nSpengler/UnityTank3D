using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : MonoBehaviour
{

    [Header("References")]
    public GameObject projectile;
    public Transform gunTransform;

    private static float _reloadTime = 3.0f;
    private float _timeSinceLastShot = _reloadTime;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        _audioSource.volume = 0.1f;
    }

    void Start()
    {
        if (!gunTransform)
        {
            Debug.LogError("No gun transform set in TankGun instance on " + gameObject.name.ToString() + ", setting gun transform to parent transform");
            gunTransform = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        _timeSinceLastShot += Time.deltaTime;
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireGun();
        }
    }

    private bool IsGunReady()
    {
        if (_timeSinceLastShot >= _reloadTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void FireGun()
    {
        if (IsGunReady())
        {
            _audioSource.Play();
            Instantiate(projectile, gunTransform.position, gunTransform.rotation);
            _timeSinceLastShot = 0.0f;
        }
        else
        {
            Debug.LogWarning("Gun not ready to fire on TankGun!");
        }
    }
}

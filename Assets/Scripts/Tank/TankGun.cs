using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGun : MonoBehaviour
{

    [Header("References")]
    public GameObject projectile;
    public Transform gunTransform;

    private AudioSource _audioSource;

    private static float _reloadTime;
    private float _timeSinceLastShot;
    
    
    private void Awake()
    {
        gunTransform = GetComponent<Transform>();
        _audioSource = GetComponentInParent<AudioSource>();
        

        if (!gunTransform)
        {
            Debug.LogError("No gun transform set in TankGun instance on " + gameObject.name.ToString() + ", setting gun transform to parent transform");
            gunTransform = gameObject.transform;
        }

        if (!projectile)
        {
            Debug.LogError("No projectile found on " + gameObject.name.ToString() + ", you probably forgot to set it in the Inspector!");
        }

        if (!_audioSource)
        {
            Debug.LogError("No Audio Source found on " + gameObject.name.ToString() + ", creating one now..");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _reloadTime = _audioSource.clip.length;
        _timeSinceLastShot = _reloadTime;
    }

    void Start()
    {
        
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

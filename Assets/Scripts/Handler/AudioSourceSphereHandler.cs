using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSphereHandler : MonoBehaviour
{
    public SphereCollider _sphereCollider;

    private AudioSource _audioSource;

    private float minRadius = 0.1f;
    private float maxRadius = 25.0f;
    private float expandRate = 0.0f;
    private float smoothTime = 0.1f;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        _sphereCollider = GetComponent<SphereCollider>();

        if (!_audioSource)
        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!_sphereCollider)
        {
            Debug.LogError("Failed to get Sphere Collider on " + gameObject.name.ToString() + ", creating one now...");
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
        }

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_audioSource.isPlaying)
        {
            Expand();
        }
        else
        {
            _sphereCollider.radius = minRadius;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    private void Expand()
    {
        var newRad = Mathf.SmoothDamp(_sphereCollider.radius, maxRadius, ref expandRate, smoothTime);
        _sphereCollider.radius = newRad;
    }

    

}

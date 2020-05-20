using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSphereHandler : MonoBehaviour
{
    [Header("References")]
    public SphereCollider sphereCollider;

    private AudioSource _audioSource;

    private float _minRadius = 0.1f;
    private float _maxRadius = 25.0f;
    private float _expandRate = 0.0f;
    private float _smoothTime = 0.1f;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();

        if (!_audioSource)
        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!sphereCollider)
        {
            Debug.LogError("Failed to get Sphere Collider on " + gameObject.name.ToString() + ", creating one now...");
            sphereCollider = gameObject.AddComponent<SphereCollider>();
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
            sphereCollider.radius = _minRadius;
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
        var newRad = Mathf.SmoothDamp(sphereCollider.radius, _maxRadius, ref _expandRate, _smoothTime);
        sphereCollider.radius = newRad;
    }

    

}

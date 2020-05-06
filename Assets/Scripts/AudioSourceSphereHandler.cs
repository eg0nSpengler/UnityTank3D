using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSphereHandler : MonoBehaviour
{
    public AudioSource _audioSource;
    public SphereCollider _sphereCollider;
    private float maxRadius = 100.0f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
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
        while (_audioSource.isPlaying)
        {
            _sphereCollider.radius += 1.0f;
            if (_sphereCollider.radius >= maxRadius)
            {
                break;
            }
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
       Debug.LogWarning(_sphereCollider.name.ToString() + " has collided with " + other.gameObject.name.ToString());
    }
}

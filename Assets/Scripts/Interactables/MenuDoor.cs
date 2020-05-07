using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDoor : MonoBehaviour
{
    [Header("References")]
    public AudioClip DoorOpenSound;
    public AudioClip DoorCloseSound;
    public Transform _toolTipTransform;

    private AudioSource _audioSource;
    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
        
        if (!_audioSource)
        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!_toolTipTransform)
        {
            Debug.LogError("Failed to get ToolTip transform on " + gameObject.name.ToString() + " using parent gameobject transform as fallback");
            _toolTipTransform = GetComponentInParent<Transform>();
        }

        _audioSource.clip = DoorOpenSound;

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        
        Debug.Log("Menu Door is being moused over!");
        Camera.current.WorldToScreenPoint(_toolTipTransform.position);
        _audioSource.clip = DoorOpenSound;
        _audioSource.Play();
        gameObject.transform.Rotate(0.0f, -45.0f, 0.0f);            
    }

    private void OnMouseExit()
    {
        _audioSource.clip = DoorCloseSound;
        _audioSource.Play();
        gameObject.transform.Rotate(0.0f, 45.0f, 0.0f);
          
    }
}

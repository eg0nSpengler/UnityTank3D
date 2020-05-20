using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankActor : MonoBehaviour
{

    [Header("References")]
    public AudioClip wallBumpAudio;

    private AudioSource _audioSource;
    private CharacterController _charController;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _charController = GetComponent<CharacterController>();

        if (!_audioSource)
        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!_charController)
        {
            Debug.LogError("Failed to get Character Controller on " + gameObject.name.ToString() + ", creating one now...");
            _charController = gameObject.AddComponent<CharacterController>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Level")
        {
            Debug.Log("Colliding with level!");
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankMovement : MonoBehaviour
{
    [Header("Variables")]
    public int tankSpeed;
    public int rotationSpeed;

    [Header("Audio References")]
    public AudioClip boostSound;

    private int boostAmount;
    private bool isBoosting; // Are we currently boosting?

    private CharacterController _charController;
    private AudioSource _audioSource;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _audioSource =  GetComponent<AudioSource>();

        if (!_charController)
        {
            Debug.LogError("Failed to get character controller in TankMovement instance on " + gameObject.name.ToString() + ", creating one now.");
            _charController = gameObject.AddComponent<CharacterController>();
        }

        if (!_audioSource)
        {
            Debug.LogError("Failed to get AudioSource in TankMovement instance on " + gameObject.name.ToString() + ", creating one now.");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (!boostSound)
        {
            Debug.LogWarning("No Boost Sound provided for TankMovement!");
        }

        isBoosting = false;
        tankSpeed = 2;
        rotationSpeed = 1;
        boostAmount = 4;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _charController.SimpleMove(transform.forward * tankSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _charController.SimpleMove((transform.forward * -1) * tankSpeed);
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            _charController.SimpleMove((transform.forward * tankSpeed * boostAmount));
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down * rotationSpeed);
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up * rotationSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}

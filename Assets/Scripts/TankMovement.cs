using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankMovement : MonoBehaviour
{
    [Header("Variables")]
    public int tankSpeed = 0;
    public int rotationSpeed = 0;

    private CharacterController _charController;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();

        if (!_charController)
        {
            Debug.LogError("Failed to get character controller in TankMovement instance on " + gameObject.name.ToString() + ", creating one now.");
            _charController = gameObject.AddComponent<CharacterController>();
        }
        tankSpeed = 2;
        rotationSpeed = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    // Update is called once per frame
    void Update()
    {
        //HandleMovement();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _charController.SimpleMove(transform.forward * tankSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            _charController.SimpleMove((transform.forward * -1) * tankSpeed);
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
}

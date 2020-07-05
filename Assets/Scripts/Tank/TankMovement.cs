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
    private enum TANK_DIR
    {
        NONE,
        FORWARD,
        BACK,
        ROTATE_LEFT,
        ROTATE_RIGHT
    }

    private CharacterController _charController;
    private AudioSource _audioSource;
    private TANK_DIR _tankDir;

    private int _boostAmount;
    private bool _isBoosting; // Are we currently boosting?
    private bool _isMoving; // Is the Tank currently moving?

    private delegate void TankBoostBegin();
    private delegate void TankBoostEnd();

    /// <summary>
    /// Called when the Tank begins boosting
    /// </summary>
    private static event TankBoostBegin OnTankBoostBegin;

    /// <summary>
    /// Called when the Tank stops boosting
    /// </summary>
    private static event TankBoostEnd OnTankBoostEnd;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();

        _isBoosting = false;
        _isMoving = false;
        tankSpeed = 2;
        rotationSpeed = 1;
        _boostAmount = 4;
        _tankDir = TANK_DIR.NONE;

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

        OnTankBoostBegin += InvokeBoostSound;
        OnTankBoostEnd += EndBoostSound;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDisable()
    {
        OnTankBoostBegin -= InvokeBoostSound;
        OnTankBoostEnd -= EndBoostSound;
    }

    private void FixedUpdate()
    {
        if (_isMoving == true)
        {
            MoveTank(_tankDir);
        }

        if (_isBoosting == true)
        {
            BoostTank();
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void MoveTank(TANK_DIR dir)
    {   
        switch(dir)
        {
            case TANK_DIR.FORWARD: _charController.SimpleMove(transform.forward * tankSpeed);
                                    _tankDir = TANK_DIR.FORWARD;
                                    break;

            case TANK_DIR.BACK: _charController.SimpleMove((transform.forward * -1) * tankSpeed);
                                 _tankDir = TANK_DIR.BACK;
                                break;

            case TANK_DIR.ROTATE_LEFT: transform.Rotate(Vector3.down * rotationSpeed);
                                        _tankDir = TANK_DIR.ROTATE_LEFT;
                                        break;

            case TANK_DIR.ROTATE_RIGHT: transform.Rotate(Vector3.up * rotationSpeed);
                                        _tankDir = TANK_DIR.ROTATE_RIGHT;
                                        break;
            default: break;

        }
        
    }

    void BoostTank()
    {
        _charController.SimpleMove((transform.forward * tankSpeed * _boostAmount));
        
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            _isMoving = true;
            MoveTank(TANK_DIR.FORWARD);
        }

        if (Input.GetKeyUp(KeyCode.W) == true)
        {
            _isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            _isMoving = true;
            MoveTank(TANK_DIR.BACK);
        }

        if (Input.GetKeyUp(KeyCode.S) == true)
        {
            _isMoving = false;
            
        }

        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            _isMoving = true;
            MoveTank(TANK_DIR.ROTATE_LEFT);
        }

        if (Input.GetKeyUp(KeyCode.A) == true)
        {
            _isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.D) == true)
        {
            _isMoving = true;
            MoveTank(TANK_DIR.ROTATE_RIGHT);
        }

        if (Input.GetKeyUp(KeyCode.D) == true)
        {
            _isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (_isBoosting == true)
            {
                _isBoosting = false;
                OnTankBoostEnd?.Invoke();
            }
            else
            {
                _isBoosting = true;
                OnTankBoostBegin?.Invoke();
            }
        }
    }

    void InvokeBoostSound()
    {
        if (_isBoosting == true)
        {
            InvokeRepeating("PlayBoostSound", 0.0f, _audioSource.clip.length);
        }
       
    }

    void EndBoostSound()
    {
        CancelInvoke();
    }

    void PlayBoostSound()
    {
       _audioSource.clip = boostSound;
       _audioSource.Play();
    }

}

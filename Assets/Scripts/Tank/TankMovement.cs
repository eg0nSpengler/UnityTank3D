using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
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
        tankSpeed = 4;
        rotationSpeed = 3;
        _boostAmount = 4;
        _tankDir = TANK_DIR.NONE;

        if (!boostSound)
        {
            Debug.LogWarning("No Boost Sound provided for TankMovement!");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PauseBoostSound());
    }

    private void OnEnable()
    {
        OnTankBoostBegin += InvokeBoostSound;
        OnTankBoostEnd += EndBoostSound;
    }
    private void OnDisable()
    {
        OnTankBoostBegin -= InvokeBoostSound;
        OnTankBoostEnd -= EndBoostSound;
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        if (GameManager.IsGamePaused == false)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.IsGamePaused == false)
        {
            HandleInput();
        }
    }

    void MoveTank(TANK_DIR dir)
    {
            switch (dir)
            {
                case TANK_DIR.FORWARD:
                    _charController.SimpleMove(transform.forward * tankSpeed);
                    _tankDir = TANK_DIR.FORWARD;
                    break;

                case TANK_DIR.BACK:
                    _charController.SimpleMove((transform.forward * -1) * tankSpeed);
                    _tankDir = TANK_DIR.BACK;
                    break;

                case TANK_DIR.ROTATE_LEFT:
                    transform.Rotate(Vector3.down * rotationSpeed);
                    _tankDir = TANK_DIR.ROTATE_LEFT;
                    break;

                case TANK_DIR.ROTATE_RIGHT:
                    transform.Rotate(Vector3.up * rotationSpeed);
                    _tankDir = TANK_DIR.ROTATE_RIGHT;
                    break;
                default: break;

            }
    }

    void BoostTank()
    {
        if (gameObject.GetComponent<HealthComponent>().IsDead == false)
        {
            _charController.SimpleMove((transform.forward * tankSpeed * _boostAmount));
        }
        
    }

    void HandleInput()
    {
        if (gameObject.GetComponent<HealthComponent>().IsDead == false)
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

            if (Input.GetKeyDown(KeyCode.LeftShift))
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

    IEnumerator PauseBoostSound()
    {
        yield return new WaitUntil(() => GameManager.IsGamePaused == true);
        _audioSource.Pause();
        EndBoostSound();
        StartCoroutine(ResumeBoostSound());
    }

    IEnumerator ResumeBoostSound()
    {
        yield return new WaitUntil(() => GameManager.IsGamePaused == false);
        _audioSource.UnPause();
        InvokeBoostSound();
        StartCoroutine(PauseBoostSound());
    }
}

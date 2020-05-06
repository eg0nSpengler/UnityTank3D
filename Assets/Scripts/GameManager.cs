using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private enum GAME_STATE 
    {
        STATE_NONE,
        STATE_PREBRIEFING,
        STATE_PLAYING,
        STATE_POSTBRIEFING,
        STATE_GAME_OVER
    }

    [Header("References")]
    public GameObject LevelPortal;

    private static List<Camera> _cameraList;
    private static List<GameObject> _sphereList;

    private static Camera _playerCamera;
    private static Camera _mainCamera;

    private static OnSphereDestroyed OnSphereDestroyedDelegate;
    private static OnPlayerEnterPortal OnPlayerEnterPortalDelegate;

    private static GAME_STATE _gameState;
     
    private int _numSpheresInLevel = 0;
    private int _numSpheresCollected = 0;
    private int _playerScore = 0;

    private delegate void OnSphereDestroyed();
    private delegate void OnPlayerEnterPortal();


    private void Awake()
    {
        _cameraList = new List<Camera>();
        _sphereList = new List<GameObject>();

        foreach (var cam in FindObjectsOfType<Camera>())
        {
            _cameraList.Add(cam);
            if (cam.name == "actorCamera")
            {
                _playerCamera = cam;
                _playerCamera.depth = 1;
            }
            else
            {
                _mainCamera = cam;
                _mainCamera.depth = 0;
            }
        }

        Debug.Log("Camera list contains " + _cameraList.Count + " cameras");


        foreach(var sphere in FindObjectsOfType<GameObject>())
        {
            if (sphere.tag == "Sphere")
            {
                _sphereList.Add(sphere);
            }
        }

        
        _numSpheresInLevel = _sphereList.Count;
       
        Debug.Log("Sphere list contains " + _sphereList.Count + " spheres");

        OnSphereDestroyedDelegate = RemoveSphereFromList;
        OnPlayerEnterPortalDelegate = LoadPostBriefingScene;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _gameState = GAME_STATE.STATE_PREBRIEFING;
        
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _mainCamera.enabled = true;
            _playerCamera.enabled = false;
        }
        else
        {
            _mainCamera.enabled = false;
            _playerCamera.enabled = true;
        }
    }

    private void RemoveSphereFromList()
    {
        var rand = Random.Range(0, _sphereList.Count);

        _numSpheresInLevel--;
        _numSpheresCollected++;
        
        _sphereList.RemoveRange(rand, 1);

        Debug.Log("The sphere list now contains " + _sphereList.Count.ToString() + " spheres");

        if (_sphereList.Count <= 0)
        {
            Debug.Log("No more spheres to remove from the SphereList!");
            TallyScore();
            ShowPortal();
        }
    }

    private void ShowPortal()
    {
        LevelPortal.SetActive(true);
    }

    private static void LoadPostBriefingScene()
    {
        Debug.Log("Loading POST_BRIEFING SCENE");
        SceneManager.LoadSceneAsync("POST_BRIEFING", LoadSceneMode.Single);
    }

    public static void InvokeSphereDestroyedDelegate()
    {
       OnSphereDestroyedDelegate.Invoke();
    }

    public static void InvokeOnPlayerEnterPortalDelegate()
    {
        OnPlayerEnterPortalDelegate.Invoke();
    }

    private void TallyScore()
    {
        _playerScore = _numSpheresCollected * 10000;

        Debug.Log("The final score is " + _playerScore.ToString());
    }
}

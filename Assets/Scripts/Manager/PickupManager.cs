using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton to manage all Pickups within a Level
/// </summary>
public class PickupManager : MonoBehaviour
{
    public delegate void PickupCollected();
    public delegate void AllPickupsCollected();
    public delegate void PortalSpawn();
    public delegate void PreBriefingLoad();
    public delegate void ScoreUpdated();

    /// <summary>
    /// Called when all pickups in a level are collected
    /// </summary>
    public static event AllPickupsCollected OnAllPickupsCollectedEvent;

    /// <summary>
    /// Called when a pickup is collected and removed from the list
    /// </summary>
    public static event PickupCollected OnPickupCollected;


    private static List<GameObject> _pickupList;
    private static List<Vector3> _pickupPosList;
    private static List<bool> _pickupBoolList;

    private static Vector3 _lastCollectedPos;
    private static bool _lastPickupBool;
    private static int _playerScore;
    private static int _numPickupsCollected;
    private static int _numPickupsLost;


    private void Awake()
    {
        _pickupList = new List<GameObject>();
        _pickupPosList = new List<Vector3>();
        _pickupBoolList = new List<bool>();

        _playerScore = 0;
        _numPickupsCollected = 0;
        _numPickupsLost = 0;
        _lastCollectedPos = new Vector3(0.0f, 0.0f, 0.0f);
        _lastPickupBool = false;

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.tag == TagStatics.GetPickupTag())
            {
                _pickupPosList.Add(obj.transform.position);
                _pickupList.Add(obj);
            }
        }

        if(_pickupList.Count <= 0)
        {
            Debug.LogWarning("PickupManager didn't find any valid pickups within the current level");
            Debug.LogWarning("Did you forget to tag any Pickup prefab instances in the scene as Pickup?");
        }


        Debug.Log("Pickup list contains " + _pickupList.Count + " pickups");

        SphereHandler.OnPickupCollectedEvent += UpdatePickupList;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDisable()
    {
        SphereHandler.OnPickupCollectedEvent -= UpdatePickupList;
    }

    private void UpdatePickupList()
    {
        
        //This container copy is done because modifying a container while iterating over it is a giant NO-NO
        //Anyways, here we just loop over each pickup in the pickup list
        foreach (var pickup in _pickupList.ToArray()) 
        {
            //When a pickup is collected, it sets itself to be inactive in the scene
            //We know the pickup that was just collected if we find an inactive Pickup in our list
            if (pickup.activeInHierarchy == false)
            {
                var pickupResult = pickup.GetComponent<SphereHandler>().GetPickupCollector();
                Debug.Log(pickupResult.ToString());
                 _lastPickupBool = pickupResult;
                _lastCollectedPos = pickup.transform.position;
                _pickupBoolList.Add(pickupResult);
                _pickupList.Remove(pickup);
                break;
            }
        }

        _numPickupsCollected++;

        Debug.Log("The Pickup list now contains " + _pickupList.Count.ToString() + " pickups");
        OnPickupCollected();

        if (_pickupList.Count <= 0)
        {
            TallyScore();
            OnAllPickupsCollectedEvent();
        }

    }

    private void TallyScore()
    {
        _playerScore = _numPickupsCollected * 10000;
        
        Debug.Log("The final score is " + _playerScore.ToString());
        Debug.Log("The player has collected " + _numPickupsCollected.ToString() + " pickups");
    }

    /// <summary>
    /// Returns the current Player Score
    /// </summary>
    /// <returns></returns>
    public static int GetPlayerScore()
    {
        return _playerScore;
    }

    /// <summary>
    /// Returns the total number of Pickups in the current Level
    /// </summary>
    public static int GetNumPickupsInLevel()
    {
        return _pickupList.Count;
    }

    /// <summary>
    /// Returns the current number of Pickups collected by the Player
    /// </summary>
    public static int GetNumCollectedPickups()
    {
        return _numPickupsCollected; 
    }

    /// <summary>
    /// Returns the current number of Pickups failed to be collected by the Player
    /// </summary>
    public static int GetNumLostPickups()
    {
        return _numPickupsLost;
    }

    /// <summary>
    /// Returns each Pickup Position in the Level
    /// </summary>
    public static IEnumerable<Vector3> GetPickupPositions()
    {
        foreach (var pos in _pickupList)
        {
            yield return pos.transform.position;
        }
    }


    /// <summary>
    /// Returns the boolean value for each pickup in the level
    /// TRUE if the player collected the pickup
    /// FALSE if a monster collected the pickup
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<bool> GetPickupBoolList()
    {
        foreach (var result in _pickupBoolList)
        {
            yield return result;
        }
    }

    /// <summary>
    /// Returns the position of the most recently collected Pickup
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetRecentCollectedPos()
    {
        return _lastCollectedPos;
    }

    /// <summary>
    /// Returns the boolean result of the most recently collected Pickup
    /// </summary>
    /// <returns></returns>
    public static bool GetRecentPickupBool()
    {
        return _lastPickupBool;
    }
}

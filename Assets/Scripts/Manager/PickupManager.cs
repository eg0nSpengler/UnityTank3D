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

    /// <summary>
    /// Called when the player score is updated at the end of a level
    /// </summary>
    public static event ScoreUpdated OnScoreUpdated;

    private static List<GameObject> _pickupList;

    private static int _playerScore = 0;
    private static int _numPickupsInLevel = 0;
    private static int _numPickupsCollected = 0;
    private static int _numPickupsLost = 0;


    private void Awake()
    {
        _pickupList = new List<GameObject>();

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.tag == "Pickup")
            {
                _pickupList.Add(obj);
                if (obj.GetComponent<PickupComponent>() == null)
                {
                    Debug.LogError(obj.name.ToString() + " in the PickupList in PickupManager doesn't have a Pickup Component, adding one to it now ...");
                    obj.AddComponent<PickupComponent>();
                }
            }
        }

        if(_pickupList.Count <= 0)
        {
            Debug.LogWarning("PickupManager didn't find any valid pickups within the current level");
            Debug.LogWarning("Did you forget to tag any Pickup prefab instances in the scene as Pickup?");
        }

        Debug.Log("Pickup list contains " + _pickupList.Count + " pickups");

        _numPickupsInLevel = _pickupList.Count;
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
        //I only remove a random pickup because we don't care about which pickup was collected
        //We only care that ANY pickup was collected
        var rand = Random.Range(0, _pickupList.Count);

        _numPickupsInLevel--;
        _numPickupsCollected++;
        _pickupList[rand].GetComponent<PickupComponent>().IsCollected = true;

        Debug.Log("The Pickup list now contains " + _pickupList.Count.ToString() + " pickups");
        OnPickupCollected();

        if (_numPickupsCollected >= _pickupList.Count)
        {
            TallyScore();
            OnAllPickupsCollectedEvent();
        }

    }

    private void TallyScore()
    {
        _playerScore = _numPickupsCollected * 10000;

        OnScoreUpdated();

        Debug.Log("The final score is " + _playerScore.ToString());
    }

    /// <summary>
    /// Returns the current Player Score
    /// </summary>
    /// <returns></returns>
    public static int GetScore()
    {
        return _playerScore;
    }

    /// <summary>
    /// Returns the total number of Pickups in the current Level
    /// </summary>
    /// <returns></returns>
    public static int GetNumPickupsInLevel()
    {
        return _pickupList.Count;
    }

    /// <summary>
    /// Returns the current number of Pickups collected by the Player
    /// </summary>
    /// <returns></returns>
    public static int GetNumCollectedPickups()
    {
        return _numPickupsCollected; 
    }

    /// <summary>
    /// Returns the current number of Pickups failed to be collected by the Player
    /// </summary>
    /// <returns></returns>
    public static int GetNumLostPickups()
    {
        return _numPickupsLost;
    }

    /// <summary>
    /// Returns each Pickup Position in the Level
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Vector3> GetPickupPositions()
    {
        foreach (var pos in _pickupList)
        {
            yield return pos.transform.position;
        }
    }
}

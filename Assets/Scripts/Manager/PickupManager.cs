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
    private int _numPickupsInLevel = 0;
    private int _numPickupsCollected = 0;


    private void Awake()
    {
        _pickupList = new List<GameObject>();

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.tag == "Pickup")
            {
                _pickupList.Add(obj);
            }
        }

        if(_pickupList.Count <= 0)
        {
            Debug.LogWarning("PickupManager didn't find any valid pickups within the current level");
            Debug.LogWarning("Did you forget to tag any Pickup prefab instances in the scene as Pickup?");
        }

        _numPickupsInLevel = _pickupList.Count;

       
        Debug.Log("Pickup list contains " + _pickupList.Count + " pickups");

        SphereHandler.OnPickupCollectedEvent += RemovePickupFromList;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void RemovePickupFromList()
    {
        //I only remove a random pickup because we don't care about which pickup was removed
        //We only care that ANY pickup was removed
        var rand = Random.Range(0, _pickupList.Count);

        _numPickupsInLevel--;
        _numPickupsCollected++;

        _pickupList.RemoveRange(rand, 1);

        Debug.Log("The Pickup list now contains " + _pickupList.Count.ToString() + " pickups");

        OnPickupCollected();

        if (_pickupList.Count <= 0)
        {
            Debug.Log("No more Pickups to remove from the PickupList!");
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

    public static int GetScore()
    {
        return _playerScore;
    }

    public static int GetNumPickupsInLevel()
    {
        return _pickupList.Count;
    }
}

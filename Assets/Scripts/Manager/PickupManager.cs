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
    /// A list of pickups in the level
    /// </summary>
    private static List<GameObject> _pickupList;

    /// <summary>
    /// A list of pickup positions in the level (used primarily for the Radar)
    /// </summary>
    private static List<Vector3> _pickupPosList;

    /// <summary>
    /// A list of boolean vals corresponding to each pickup in the level
    /// <para>TRUE means the pickup was collected by the Player</para>
    /// <para>FALSE means that a monster collected it or the Player shot it</para>
    /// </summary>
    private static List<bool> _pickupBoolList;

    /// <summary>
    /// The position of the most recent collected Pickup in the Level
    /// </summary>
    public static Vector3 LastCollectedPos { private set; get; }

    /// <summary>
    /// A boolean for the most recent collected Pickup in the Level
    /// <para>TRUE if collected by the Player</para>
    /// <para>FALSE if collected by a Monster (or shot by the Player)</para>
    /// </summary>
    public static bool LastPickupBool { private set; get; }

    /// <summary>
    /// The current Player score
    /// </summary>
    public static int PlayerScore { private set; get; }

    /// <summary>
    /// The number of pickups collected by the Player
    /// </summary>
    public static int NumPickupsCollected { private set; get; }

    /// <summary>
    /// The number of pickups lost by the Player
    /// <para>Monsters consuming pickups or the Player shooting a pickup will count towards this</para>
    /// </summary>
    public static int NumPickupsLost { private set; get; }

    /// <summary>
    /// The number of pickups currently in the Level
    /// </summary>
    public static int NumPickupsInLevel { private set; get; }

    private void Awake()
    {
        _pickupList = new List<GameObject>();
        _pickupPosList = new List<Vector3>();
        _pickupBoolList = new List<bool>();

        PlayerScore = 0;
        NumPickupsCollected = 0;
        NumPickupsLost = 0;
        LastCollectedPos = new Vector3(0.0f, 0.0f, 0.0f);
        LastPickupBool = false;

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.tag == TagStatics.GetPickupTag())
            {
                _pickupPosList.Add(obj.transform.position);
                _pickupList.Add(obj);
                obj.GetComponent<SphereHandler>().OnPickupCollectedEvent += UpdatePickupList;
            }
        }

        NumPickupsInLevel = _pickupList.Count;

        if(_pickupList.Count <= 0)
        {
            Debug.LogWarning("PickupManager didn't find any valid pickups within the current level");
            Debug.LogWarning("Did you forget to tag any Pickup prefab instances in the scene as Pickup?");
        }


        Debug.Log("Pickup list contains " + _pickupList.Count + " pickups");


    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {
        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.tag == TagStatics.GetPickupTag())
            {
                obj.GetComponent<SphereHandler>().OnPickupCollectedEvent -= UpdatePickupList;
            }
        }
    }

    private void UpdatePickupList()
    {
        //This container copy is done because modifying a container while iterating over it is a giant NO-NO
        //Anyways, here we just loop over each pickup in the pickup list
        foreach (var pickup in _pickupList.ToArray()) 
        {

            if (pickup.GetComponent<SphereHandler>().IsCollected == true)
            {
                var pickupResult = pickup.GetComponent<SphereHandler>().PickupCollector;

                if (pickupResult == true)
                {
                    //Player collected the pickup
                    NumPickupsCollected++;
                }
                else
                {
                    //Monster got the pickup or the player shot it
                    NumPickupsLost++;
                }

                 LastPickupBool = pickupResult;
                LastCollectedPos = pickup.transform.position;
                _pickupBoolList.Add(pickupResult);
                _pickupList.Remove(pickup);
                break;
            }
        }

        OnPickupCollected?.Invoke();

        Debug.Log("The Pickup list now contains " + _pickupList.Count.ToString() + " pickups");

        if (_pickupList.Count <= 0)
        {
            TallyScore();
            OnAllPickupsCollectedEvent?.Invoke();
        }

    }

    private void TallyScore()
    {
        PlayerScore = NumPickupsCollected * 10000;
        
        Debug.Log("The final score is " + PlayerScore.ToString());
        Debug.Log("The player has collected " + NumPickupsCollected.ToString() + " pickups");
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
    /// <para>TRUE if the player collected the pickup</para>
    /// <para>FALSE if a monster collected the pickup (Or if the player shot a pickup) </para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<bool> GetPickupBoolList()
    {
        foreach (var result in _pickupBoolList)
        {
            yield return result;
        }
    }

}

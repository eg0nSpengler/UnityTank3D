using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton to manage all Pickups within a Level
/// </summary>
public class PickupManager : MonoBehaviour
{
    private static List<GameObject> _pickupList;

    private int _numPickupsInLevel = 0;
    private int _numPickupsCollected = 0;

    private static OnPickupDestroyed OnPickupDestroyedDelegate;
    private static OnPlayerEnterPortal OnPlayerEnterPortalDelegate;
    private static OnPreBriefingLoad OnPreBriefingLoadDelegate;

    private delegate void OnPickupDestroyed();
    private delegate void OnPlayerEnterPortal();
    private delegate void OnPreBriefingLoad();


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

        OnPickupDestroyedDelegate = RemovePickupFromList;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void RemovePickupFromList()
    {
        var rand = Random.Range(0, _pickupList.Count);

        _numPickupsInLevel--;
        _numPickupsCollected++;

        _pickupList.RemoveRange(rand, 1);

        Debug.Log("The Pickup list now contains " + _pickupList.Count.ToString() + " pickups");

        if (_pickupList.Count <= 0)
        {
            Debug.Log("No more Pickups to remove from the PickupList!");
            TallyScore();
            //ShowPortal();
        }

    }

    private void TallyScore()
    {
        //_playerScore = _numPickupsCollected * 10000;

        //Debug.Log("The final score is " + _playerScore.ToString());
    }

    //This will inform the PickupManager to remove our pickup from the pickup list
    public static void InvokePickupDestroyedDelegate(GameObject obj)
    {
        if (obj.tag == "Pickup")
        {
            OnPickupDestroyedDelegate.Invoke();
        }
        else
        {
            Debug.LogError("Failed to invoke PickupDestroyed Delegate in the Game Manager, you probably tried to invoke this delegate from the wrong GameObject");
        }

    }
    
}

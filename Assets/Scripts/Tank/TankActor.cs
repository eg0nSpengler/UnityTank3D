using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This just serves as a class to hold references to other components on our TankActor
/// </summary>
public class TankActor : MonoBehaviour
{
    public HealthComponent healthComp { get; private set; }

    public int playerScore { get; private set; }
    public int levelNum { get; private set; }

    public int numPickupsCollected { get; private set; }

    public int numPickupsLost { get; private set; }

    private  int _numPickupsCollected;

    private void Awake()
    {
        healthComp = GetComponent<HealthComponent>();

        if (!healthComp)
        {
            Debug.LogError("Failed to get Health Component on " + gameObject.name.ToString() + ", creating one now...");
            gameObject.AddComponent<HealthComponent>();
        }

        LevelPortal.OnLevelPortalEnabled += SaveTankData;
        _numPickupsCollected = numPickupsCollected;
        
    }


    public void SaveTankData()
    {
        GameDataSerializer.SaveGameData(this);
    }

    public int GetTankPickups()
    {
        return _numPickupsCollected;
    }
}

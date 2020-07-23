using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int playerScore;
    public int playerHealth;
    public int numPickupsCollected;
    public int numPickupsLost;
    public int numPickupsTotal;
    public int levelNum;
    public int finalLevelTime;
    public IEnumerable<bool> pickupBool;

    public GameData(TankActor.TankStats tankStats)
    {
        playerScore = tankStats.PlayerScore;
        numPickupsCollected = tankStats.NumPickupsCollected;
        numPickupsLost = tankStats.NumPickupsLost;
        numPickupsTotal = tankStats.NumPickupsTotal;
        levelNum = tankStats.LevelNum;
        pickupBool = tankStats.PickupBool;
        finalLevelTime = tankStats.FinalLevelTime;

    }
}

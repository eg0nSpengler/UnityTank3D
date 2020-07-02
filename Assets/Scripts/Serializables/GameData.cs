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

    public GameData(TankActor tankActor)
    {
        playerScore = PickupManager.PlayerScore;
        playerHealth = tankActor.healthComp.CurrentHP;
        numPickupsCollected = PickupManager.NumPickupsCollected;
        numPickupsLost = PickupManager.NumPickupsLost;
        numPickupsTotal = PickupManager.NumPickupsInLevel;
        levelNum = LevelManager.CurrentLevelStats.CurrentLevelNum;

    }
}

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
        playerScore = PickupManager.GetPlayerScore();
        playerHealth = tankActor.healthComp.CurrentHP;
        numPickupsCollected = PickupManager.GetNumCollectedPickups();
        numPickupsLost = PickupManager.GetNumLostPickups();
        numPickupsTotal = PickupManager.GetNumPickupsInLevel();
        levelNum = LevelManager.GetLevelNum();

    }
}

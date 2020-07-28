using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for data serialization
/// </summary>
public static class GameDataSerializer
{

    public static List<GameData> _gameDataList = new List<GameData>();

    /// <summary>
    /// This is meant to only be called once when the game begins
    /// </summary>
    public static void InitGameData()
    {
        if (_gameDataList.Count > 0)
        {
            Debug.LogError("InitGameData failed due to pre-existing initialized data...");
            return;
        }
        else
        {
            TankActor.TankStats tankStats;
            tankStats = new TankActor.TankStats();
            GameData gmData = new GameData(tankStats);
            _gameDataList.Add(gmData);
            Debug.Log("InitGameData called");
        }
    }


    /// <summary>
    /// Saves the current Game Data
    /// </summary>
    /// <param name="tankStats"></param>
    public static void SaveGameData(TankActor.TankStats tankStats)
    {
        GameData gmData = new GameData(tankStats);
        var json = JsonUtility.ToJson(gmData);

        if (_gameDataList.Count > 0)
        {
            var end = _gameDataList.Count - 1;
            // This is just to make sure that there is always only one copy of GameData
            _gameDataList[end] = gmData;
        }
        else
        {
            _gameDataList.Add(gmData);
        }
    }

    /// <summary>
    /// Loads Game Data
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static GameData LoadGameData(int index)
    {
        if (_gameDataList[index] == null)
        {
            return null;
        }
        else
        {
            return _gameDataList[index];
        }
    }

    
    /// <summary>
    /// This is called when the user quits back to the main menu
    /// </summary>
    public static void ResetGameData()
    {
        _gameDataList.Clear();
        InitGameData();
    }
}

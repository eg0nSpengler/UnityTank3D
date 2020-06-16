using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class GameDataSerializer
{
    private static List<GameData> _gameDataList = new List<GameData>();

    public static void SaveGameData(TankActor tankActor)
    {
        GameData gmData = new GameData(tankActor);
        var json = JsonUtility.ToJson(gmData);
        _gameDataList.Add(gmData);
    }

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

}

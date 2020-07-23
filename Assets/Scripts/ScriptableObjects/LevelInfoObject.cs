using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfoObject", menuName = "Level Info Object", order = 52)]
public class LevelInfoObject : ScriptableObject
{
    public LevelInfo[] levelInfoList;

    /// <summary>
    /// Returns each Level Info in the LevelInfoList
    /// </summary>
    /// <returns>Level Info</returns>
    public IEnumerable<LevelInfo> GetLevelInfoList()
    {
        foreach (var info in levelInfoList)
        {
            yield return info;
        }
    }

    /// <summary>
    /// Returns the Level Info for the level number provided
    /// </summary>
    /// <param name="lvlNum">The number of the level</param>
    /// <returns>Level Info</returns>
    public LevelInfo GetLevelInfo(int lvlNum)
    {
        var lvlInfo = levelInfoList[lvlNum];

        if (lvlInfo)
        {
            return lvlInfo;
        }
        else
        {
            Debug.LogError("Failed to return the Level Info object for Level " + lvlNum.ToString());
            Debug.LogError("Double check the LevelInfoObject to make sure there is an assigned reference in the List");
            return null;
        }
    }

}

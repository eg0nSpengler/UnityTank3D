using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfo", menuName = "Level Info", order = 51)]
public class LevelInfo : ScriptableObject
{

    [SerializeField]
    private string LevelName;

    [SerializeField]
    private int LevelNum;

    [SerializeField]
    private string LevelDescription;

    public string GetLevelName
    {
        get
        {
            return LevelName;
        }
    }

    public string GetLevelDesc
    {
        get 
        {
            return LevelDescription;
        }
    }

    public int GetLevelNum
    {
        get
        {
            return LevelNum;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelInfo", menuName = "Level Info", order = 51)]
public class LevelInfo : ScriptableObject
{

    [SerializeField]
    private string _levelName;

    [SerializeField]
    private int _levelNum;

    [SerializeField]
    private int _levelTime;

    [SerializeField]
    private string _levelDescription;


    public string GetLevelName
    {
        get
        {
            return _levelName;
        }
    }

    public string GetLevelDesc
    {
        get 
        {
            return _levelDescription;
        }
    }

    public int GetLevelNum
    {
        get
        {
            return _levelNum;
        }
    }

    public int GetLevelTime
    {
        get 
        {
            return _levelTime;
        }
    }
}

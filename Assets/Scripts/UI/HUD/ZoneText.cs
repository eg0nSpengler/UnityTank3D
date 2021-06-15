using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TextMeshProUGUI))]
public class ZoneText : MonoBehaviour
{
    [Header("References")]
    public LevelInfo lvlInfo;

    private TextMeshProUGUI _zoneText;

    private string _zoneString;
    
    void Awake()
    {
        _zoneText = GetComponent<TextMeshProUGUI>();

        _zoneString = "Zone ";
        _zoneText.color = Color.white;

    }
    // Start is called before the first frame update
    void Start()
    {
        var currLvlInfo = LevelManager.GetLevelInfo();
        var lvlNum = currLvlInfo.GetLevelNum;

        //This is done because internally, the level numbers are for container/iteration purposes
        //We just add one to the current level number to represent the zone number

        if (lvlNum == 0)
        {
            lvlNum = 1;
        }
        else
        {
            lvlNum++;
        }

        switch (gameObject.name.ToString())
        {
            case "TITLE":
                _zoneText.text = _zoneString += lvlNum + ": " + currLvlInfo.GetLevelName;
                break;

            case "DESCRIPTION":
                _zoneText.text = currLvlInfo.GetLevelDesc;
                break;

            default:
                break;

        }
    }
    

}

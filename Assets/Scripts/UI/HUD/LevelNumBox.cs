using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelNumBox : MonoBehaviour
{
    private TextMeshProUGUI _levelNum;

    private void Awake()
    {
        _levelNum = GetComponent<TextMeshProUGUI>();
        _levelNum.color = Color.green;

        if (!_levelNum)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _levelNum = gameObject.AddComponent<TextMeshProUGUI>();
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        var currLvlInfo = LevelManager.GetLevelInfo();

        //This is done because internally, the level numbers are for container/iteration purposes
        //We just add one to the current level number to represent the zone number

        if (currLvlInfo.GetLevelNum == 0)
        {
            _levelNum.text = currLvlInfo.GetLevelNum + "1";
        }
        else
        {
            var lvlNum = currLvlInfo.GetLevelNum + 1;
            _levelNum.text = "0" + lvlNum.ToString();
        }
    }

    
}

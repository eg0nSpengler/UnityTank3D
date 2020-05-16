using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneText : MonoBehaviour
{
    [Header("References")]
    public LevelInfo _lvlInfo;

    private TextMeshProUGUI _zoneText;

    private string _zoneString = "Zone ";
    
    void Awake()
    {
        _zoneText = GetComponent<TextMeshProUGUI>();
        if (!_zoneText)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _zoneText = gameObject.AddComponent<TextMeshProUGUI>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (gameObject.name.ToString())
        {
            case "TITLE":
                _zoneText.text = _zoneString += _lvlInfo.GetLevelNum + ": " + _lvlInfo.GetLevelName;
                break;

            case "DESCRIPTION":
                _zoneText.text = _lvlInfo.GetLevelDesc;
                break;

            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

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

        if (!_levelNum)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _levelNum = gameObject.AddComponent<TextMeshProUGUI>();
        }

        _levelNum.color = Color.green;
    }
    // Start is called before the first frame update
    void Start()
    {
        _levelNum.text = "0" + LevelManager.GetLevelNum().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

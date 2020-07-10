using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupsLostText : MonoBehaviour
{
    private TextMeshProUGUI _txt;

    private void Awake()
    {
        _txt = GetComponent<TextMeshProUGUI>();
        if (!_txt)
        {
            Debug.LogError("Failed to get TextMeshProUGUI on " + gameObject.name.ToString() + ", creating one now...");
            _txt = gameObject.AddComponent<TextMeshProUGUI>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);
        _txt.text += " " + gmData.numPickupsLost;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

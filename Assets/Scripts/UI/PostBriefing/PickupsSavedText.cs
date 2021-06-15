using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(TextMeshProUGUI))]
public class PickupsSavedText : MonoBehaviour
{
    private TextMeshProUGUI _txt;

    private void Awake()
    {
        _txt = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);
        _txt.text += " " + gmData.numPickupsCollected;
    }

}

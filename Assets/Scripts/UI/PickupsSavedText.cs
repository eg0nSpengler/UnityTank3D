using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupsSavedText : MonoBehaviour
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
        _txt.text += " " + PickupManager.GetNumCollectedPickups().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

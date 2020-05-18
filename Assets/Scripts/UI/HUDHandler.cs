using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDHandler : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI levelNumBox;


    private void Awake()
    {
        if (!levelNumBox)
        {
            Debug.LogError("No UI object set for LevelNumBox in HUDHandler");
            Debug.LogError("Did you forget to provide a reference in the Inspector?");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

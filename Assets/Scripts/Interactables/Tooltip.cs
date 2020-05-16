using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text toolTipText;
    
    private void Awake()
    {
        toolTipText = GetComponent<Text>();
       
        if (!toolTipText)
        {
            Debug.LogError("No valid Text provided on " + gameObject.name.ToString() + ", creating one now...");
            toolTipText = gameObject.AddComponent<Text>();
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

    private void DisplayToolTip()
    {
        
    }

    private void HideToolTip()
    {

    }


}

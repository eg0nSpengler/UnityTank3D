using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Button References")]
    public GameObject BackButtonObj;

    private List<GameObject> _buttonList;

    private void Awake()
    {
        _buttonList = new List<GameObject>();

        if (!BackButtonObj)
        {
            Debug.LogWarning("SettingsPanel is missing a Button reference!");
        }

        _buttonList.Add(BackButtonObj);

        gameObject.SetActive(false);
        
    }

    private void OnDisable()
    {
        foreach (var button in _buttonList)
        {
            button.SetActive(false);
        }
    }
    private void OnEnable()
    {
        foreach (var button in _buttonList)
        {
            button.SetActive(true);
        }
    }
}

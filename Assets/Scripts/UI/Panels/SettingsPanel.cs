using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [Header("Button References")]
    public GameObject BackButtonObj;
    public GameObject AudioButtonObj;

    [Header("Panel References")]
    public GameObject AudioPanel;

    private List<GameObject> _buttonList;
    private List<GameObject> _panelList;

    private void Awake()
    {
        _buttonList = new List<GameObject>();
        _panelList = new List<GameObject>();

        if (!BackButtonObj || !AudioButtonObj)
        {
            Debug.LogWarning("SettingsPanel is missing a Button reference!");
        }

        if (!AudioPanel)
        {
            Debug.LogWarning("SettingsPanel is missing a Panel reference!");
        }

        _buttonList.Add(BackButtonObj);
        _buttonList.Add(AudioButtonObj);

        _panelList.Add(AudioPanel);

        AudioButton.OnAudioButtonClick += ShowAudioPanel;
    }

    private void OnDestroy()
    {
        AudioButton.OnAudioButtonClick -= ShowAudioPanel;
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

    private void ShowAudioPanel()
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(true);
        }
    }

    private void HideAudioPanel()
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(false);
        }
    }
}

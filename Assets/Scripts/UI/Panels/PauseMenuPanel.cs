using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuPanel : MonoBehaviour
{
    [Header("Button References")]
    public GameObject ExitToMenuButton;
    public GameObject ControlButton;

    [Header("Panel References")]
    public GameObject ControlsPanel;

    private List<GameObject> _buttonList;
    private List<GameObject> _panelList;

    private void Awake()
    {
        _buttonList = new List<GameObject>();
        _panelList = new List<GameObject>();

        if (!ExitToMenuButton || !ControlButton)
        {
            Debug.LogWarning("PauseMenuPanel is missing a Button reference!");
        }

        if (!ControlsPanel)
        {
            Debug.LogWarning("PauseMenuPanel is missing a Panel reference!");
        }

        _buttonList.Add(ExitToMenuButton);
        _buttonList.Add(ControlButton);

        _panelList.Add(ControlsPanel);

        gameObject.SetActive(false);

        ControlsButton.OnControlsButtonClick += DisplayControlsPanel;
        BackButton.OnBackButtonClick += HideControlsPanel;
    }

    private void OnDisable()
    {
        foreach (var button in _buttonList)
        {
            button.SetActive(false);
        }

    }

    private void OnDestroy()
    {
        ControlsButton.OnControlsButtonClick -= DisplayControlsPanel;
        BackButton.OnBackButtonClick -= HideControlsPanel;
    }

    private void OnEnable()
    {
        foreach (var button in _buttonList)
        {
            button.SetActive(true);
        }

        foreach (var panel in _panelList)
        {
            panel.SetActive(false);
        }
    }

    private void DisableButtons()
    {
        foreach (var button in _buttonList)
        {
            button.SetActive(false);
        }
    }

    private void EnableButtons()
    {
        foreach (var button in _buttonList)
        {
            button.SetActive(true);
        }
    }

    private void DisplayControlsPanel()
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(true);
        }

        DisableButtons();
    }

    private void HideControlsPanel()
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(false);
        }

        EnableButtons();
    }

}

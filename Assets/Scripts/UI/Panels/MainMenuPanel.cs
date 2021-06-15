using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuPanel : MonoBehaviour
{
    [Header("Button References")]
    public GameObject StartButton;
    public GameObject SettingsBackButton;
    public GameObject CreditsBackButton;
    public GameObject PlayLevelButton;
    public GameObject ExitGameButton;
    public GameObject SettingsButton;
    public GameObject CreditsButton;

    [Header("Panel References")]
    public GameObject SettingsPanel;
    public GameObject AudioPanel;
    public GameObject CreditsPanel;

    private List<GameObject> _menuButtons;
    private List<GameObject> _menuPanels;

    private void Awake()
    {
        _menuButtons = new List<GameObject>();
        _menuPanels = new List<GameObject>();

        if (!StartButton || !SettingsBackButton || !CreditsBackButton || !PlayLevelButton || !ExitGameButton || !SettingsButton || !CreditsButton)
        {
            Debug.LogWarning("MainMenuPanel is missing a Button reference!");
        }

        if (!SettingsPanel || !AudioPanel || !CreditsPanel)
        {
            Debug.LogWarning("MainMenuPanel is missing a Panel reference!");
        }

        _menuButtons.Add(StartButton);
        _menuButtons.Add(PlayLevelButton);
        _menuButtons.Add(ExitGameButton);
        _menuButtons.Add(SettingsButton);

        _menuPanels.Add(SettingsPanel);
        _menuPanels.Add(AudioPanel);
        _menuPanels.Add(CreditsPanel);

        SettingsBackButton.GetComponent<Button>().onClick.AddListener(EnableButtons);
        CreditsBackButton.GetComponent<Button>().onClick.AddListener(EnableButtons);
        CreditsButton.GetComponent<Button>().onClick.AddListener(ShowCreditsPanel);
        SettingsMenuButton.OnSettingsButtonClick += DisableButtons;
    }

    private void OnEnable()
    {
        foreach (var panel in _menuPanels)
        {
            panel.SetActive(false);
        }
    }

    private void OnDisable()
    {
        SettingsMenuButton.OnSettingsButtonClick -= DisableButtons;
        SettingsBackButton.GetComponent<Button>().onClick.RemoveAllListeners();
        CreditsButton.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    void DisableButtons()
    {
        // When the settings button is clicked
        // We simply disable all the buttons on the main menu panel
        // Stops them from being displayed and processing mouse events

        foreach (var button in _menuButtons)
        {
            button.SetActive(false);
        }

        //This enables the Settings panel
        EnablePanel(SettingsPanel);
    }

    void EnableButtons()
    {
        //This disables the Settings panel
        DisablePanel(SettingsPanel);
        DisablePanel(CreditsPanel);

        //Settings panel is now disabled
        //Let's re enable our buttons
        //So we can display them and have them process mouse events again

        foreach (var button in _menuButtons)
        {
            button.SetActive(true);
        }
    }

    void EnablePanel(GameObject panelToEnable)
    {
        foreach (var panel in _menuPanels)
        {
            if (panel == panelToEnable)
            {
                panel.SetActive(true);
            }
        }
    }

    void DisablePanel(GameObject panelToDisable)
    {
        foreach (var panel in _menuPanels)
        {
            if (panel == panelToDisable)
            {
                panel.SetActive(false);
            }
        }
    }

    void ShowCreditsPanel()
    {

        foreach (var button in _menuButtons)
        {
            button.SetActive(false);
        }

        EnablePanel(CreditsPanel);
    }
}

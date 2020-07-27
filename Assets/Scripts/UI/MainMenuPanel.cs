using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [Header("Button References")]
    public GameObject StartButton;
    public GameObject PlayLevelButton;
    public GameObject ExitGameButton;
    public GameObject SettingsButton;

    [Header("Panel References")]
    public GameObject SettingsPanel;

    private List<GameObject> _menuButtons;
    private List<GameObject> _menuPanels;

    private void Awake()
    {
        _menuButtons = new List<GameObject>();
        _menuPanels = new List<GameObject>();

        if (!StartButton || !PlayLevelButton || !ExitGameButton || !SettingsButton)
        {
            Debug.LogWarning("MainMenuPanel is missing a Button reference!");
        }

        if (!SettingsPanel)
        {
            Debug.LogWarning("MainMenuPanel is missing a Panel reference!");
        }

        _menuButtons.Add(StartButton);
        _menuButtons.Add(PlayLevelButton);
        _menuButtons.Add(ExitGameButton);
        _menuButtons.Add(SettingsButton);

        _menuPanels.Add(SettingsPanel);

        SettingsMenuButton.OnSettingsButtonClick += DisableButtons;
        BackButton.OnBackButtonClick += EnableButtons;
    }

    private void OnDisable()
    {
        SettingsMenuButton.OnSettingsButtonClick -= DisableButtons;
        BackButton.OnBackButtonClick -= EnableButtons;
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
        EnablePanel();
    }

    void EnableButtons()
    {
        //This disables the Settings panel
        DisablePanel();

        //Settings panel is now disabled
        //Let's re enable our buttons
        //So we can display them and have them process mouse events again

        foreach (var button in _menuButtons)
        {
            button.SetActive(true);
        }
    }

    void EnablePanel()
    {
        foreach (var panel in _menuPanels)
        {
            panel.SetActive(true);
        }
    }

    void DisablePanel()
    {
        foreach (var panel in _menuPanels)
        {
            panel.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject PauseMenuPanel;


    private List<GameObject> _panelList;

    private void Awake()
    {
        _panelList = new List<GameObject>();

        if (!PauseMenuPanel)
        {
            Debug.LogWarning("GameCanvas is missing a Panel reference!");
        }

        _panelList.Add(PauseMenuPanel);

        GameManager.OnGamePause += OpenPausePanel;
        GameManager.OnGameResume += ClosePausePanel;
    }

    private void OnDisable()
    {
        GameManager.OnGamePause -= OpenPausePanel;
        GameManager.OnGameResume -= ClosePausePanel;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(false);
        }
    }

    void OpenPausePanel()
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(true);
        }
    }

    void ClosePausePanel()
    {
        foreach (var panel in _panelList)
        {
            panel.SetActive(false);
        }
    }
}

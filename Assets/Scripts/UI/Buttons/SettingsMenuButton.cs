using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SettingsMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public delegate void SettingsButtonClick();

    /// <summary>
    /// Called when the user clicks the Settings Button
    /// </summary>
    public static event SettingsButtonClick OnSettingsButtonClick;

    private Button _settingsButton;
    private Color _defaultColor;

    void Awake()
    {
        _settingsButton = GetComponent<Button>();
        _defaultColor = _settingsButton.image.color;

        if(!_settingsButton)
        {
            Debug.LogError("Failed to get Settings Button on SettingsMenuButton!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSettingsButtonClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _settingsButton.image.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _settingsButton.image.color = _defaultColor;
    }
}

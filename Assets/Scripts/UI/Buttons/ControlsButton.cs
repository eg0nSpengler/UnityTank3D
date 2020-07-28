using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ControlsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void ControlButtonClick();

    /// <summary>
    /// Called when the user clicks the "Controls" button in the pause menu
    /// </summary>
    public static event ControlButtonClick OnControlsButtonClick;

    private Button _controlsButton;
    private Color _defaultColor;

    void Awake()
    {
        _controlsButton = GetComponent<Button>();
        _defaultColor = _controlsButton.image.color;

        if (!_controlsButton)
        {
            Debug.LogError("Failed to get Button on ControlsButton!");
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnControlsButtonClick?.Invoke();   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _controlsButton.image.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _controlsButton.image.color = _defaultColor;
    }

    private void OnEnable()
    {
        // This is done since after the user clicks the Back button on a secondary panel
        // And is brought back to the primary pause panel
        // The control button is colored as if it's encountered an OnPointerEnter event
        // Even though when this button becomes enabled again
        // The cursor is nowhere NEAR the raycast target for the button
        // Very odd, but this fixes the issue
        _controlsButton.image.color = _defaultColor;
    }
}

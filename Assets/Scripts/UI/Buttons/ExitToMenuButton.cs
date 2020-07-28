using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitToMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public delegate void ExitToMenu();

    /// <summary>
    /// Called when the user clicks the "Exit To Menu" button in the pause menu
    /// </summary>
    public static event ExitToMenu OnExitToMenuRequest;

    private Button _exitToMenuButton;
    private Color _defaultColor;

    void Awake()
    {
        _exitToMenuButton = GetComponent<Button>();
        _defaultColor = _exitToMenuButton.image.color;
        
        if (!_exitToMenuButton)
        {
            Debug.LogError("Failed to get Button on ExitToMenuButton!");
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnExitToMenuRequest?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _exitToMenuButton.image.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _exitToMenuButton.image.color = _defaultColor;
    }
}

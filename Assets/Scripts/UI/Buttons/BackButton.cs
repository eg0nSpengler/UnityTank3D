using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public delegate void BackButtonClick();

    /// <summary>
    /// Called when the user clicks the Back Button
    /// </summary>
    public static event BackButtonClick OnBackButtonClick;

    private Button _backButton;
    private Color _defaultColor;

    void Awake()
    {
        _backButton = GetComponent<Button>();
        _defaultColor = _backButton.image.color;

        if(!_backButton)
        {
            Debug.LogError("Failed to get Back Button on BackButton!");
        }
    }

    void OnDisable()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBackButtonClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _backButton.image.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _backButton.image.color = _defaultColor;
    }

}

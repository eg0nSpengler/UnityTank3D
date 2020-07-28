using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExitGameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button _exitButton;
    private Color _defaultColor;

    void Awake()
    {
        _exitButton = GetComponent<Button>();
        _defaultColor = _exitButton.image.color;
        
        if(!_exitButton)
        {
            Debug.LogError("Failed to get Button on ExitLevelButton");
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _exitButton.image.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _exitButton.image.color = _defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class StartLevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public delegate void StartLevel();

    /// <summary>
    /// Called when the user clicks on the Start Level button
    /// </summary>
    public static event StartLevel OnLevelStartEvent;

    private Button _startButton;
    private Color _defaultColor;

    private void Awake()
    {
        _startButton = GetComponent<Button>();
        _defaultColor = _startButton.image.color;

        if (!_startButton)
        {
            Debug.LogError("Failed to get Button on StartLevelButton, creating one now...");
            _startButton = gameObject.AddComponent<Button>();
            _defaultColor = _startButton.image.color;
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        _startButton.onClick.AddListener(HandleMouse);
    }


    private void HandleMouse()
    {
        OnLevelStartEvent?.Invoke();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _startButton.image.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _startButton.image.color = _defaultColor;
    }
}

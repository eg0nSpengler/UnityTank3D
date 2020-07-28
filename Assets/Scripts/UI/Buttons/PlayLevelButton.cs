using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlayLevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _playLevelButton;
    private TMP_InputField _input;
    private Color _defaultColor;

    private int _charLimit;
    private static string _savedNum;

    public delegate void PlayLevel();
    public delegate void LevelNumProvided();

    /// <summary>
    /// Called when the user clicks on the Play Level button
    /// </summary>
    public static event PlayLevel OnPlayLevel;

    /// <summary>
    /// Called when there is valid text within the level number box
    /// </summary>
    public static event LevelNumProvided OnLevelNumProvided;

    private void Awake()
    {
        _playLevelButton = GetComponent<Button>();
        _input = GetComponentInChildren<TMP_InputField>();
        _charLimit = 2;
        _savedNum = "";
        _defaultColor = _playLevelButton.image.color;


        if (!_playLevelButton)
        {
            Debug.LogError("Failed to get Play Level Button on PlayLevelButton!");
        }

        if (!_input)
        {
            Debug.LogError("Failed to get Input Field on PlayLevelButton!");
        }

        OnPlayLevel += ClearInput;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckTextForChar());
        StartCoroutine(CheckTextLength());
        ClearInput();
    }

    private void OnDisable()
    {
        OnPlayLevel -= ClearInput;
        StopAllCoroutines();
    }

    private void Start()
    {
        _playLevelButton.onClick.AddListener(HandleMouse);
    }

    /// <summary>
    /// Checks the input field for characters and resets the field if any are detected
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckTextForChar()
    {
        yield return new WaitWhile(() => _input.text.Length <= 0);

        foreach (var c in _input.text)
        {
            if (char.IsDigit(c) == false)
            {
                Debug.LogWarning("Character detected!");
                Debug.LogWarning("Please enter a number...");
                StartCoroutine(FlashBox());
                _input.text = "";
            }
        }

        _savedNum = _input.text;

        StartCoroutine(CheckTextForChar());
    }

    /// <summary>
    /// Checks the length of the input field to see if it is >= 2 characters
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckTextLength()
    {
        // Level numbers can be 2 digits MAX
        yield return new WaitWhile(() => _input.text.Length <= _charLimit);
        _input.text = "";
        Debug.LogWarning("Char limit reached!");
        StartCoroutine(FlashBox());
        StartCoroutine(CheckTextLength());
    }

    private void HandleMouse()
    {
        OnPlayLevel?.Invoke();
    }

    /// <summary>
    /// Clears the input field
    /// </summary>
    private void ClearInput()
    {
        // I know there is a builtin string method to remove chars from a string
        // But using that here forces the user to backspace/click the input box to enter text again
        // This makes it to whereas they can just immediately provide new input
        _input.text = "";
        _input.image.color = _defaultColor;
    }

    /// <summary>
    /// Returns the number entered into the input box
    /// </summary>
    /// <returns></returns>
    public static int GetNum()
    {

        if (_savedNum.Length > 0)
        {
            var numConv = _savedNum;

            //Parses the string for integers and returns an int accordingly
            return int.Parse(numConv, System.Globalization.NumberStyles.Integer);
        }
        else
        {
            return 0;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _playLevelButton.image.color = Color.green;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _playLevelButton.image.color = _defaultColor;
    }

    /// <summary>
    /// Changes the input box color to Red for half a second
    /// This is done to communicate to the user that they've provided invalid text to the input field
    /// </summary>
    /// <returns></returns>
    IEnumerator FlashBox()
    {
        _input.image.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _input.image.color = _defaultColor;
    }
}

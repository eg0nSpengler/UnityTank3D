using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayLevelButton : MonoBehaviour
{
    private Button _playLevelButton;
    private TMP_InputField _input;

    private int _charLimit;
    private static string _savedNum;

    public delegate void PlayLevel();

    /// <summary>
    /// Called when the user clicks on the Play Level button
    /// </summary>
    public static event PlayLevel OnPlayLevel;

    private void Awake()
    {
        _playLevelButton = GetComponent<Button>();
        _input = GetComponentInChildren<TMP_InputField>();
        _charLimit = 2;
        _savedNum = "";


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


    private void OnDisable()
    {
        OnPlayLevel -= ClearInput;
        StopAllCoroutines();
    }

    private void Start()
    {
        _playLevelButton.onClick.AddListener(HandleMouse);
        StartCoroutine(CheckTextForChar());
        StartCoroutine(CheckTextLength());
    }

    private void Update()
    {

    }

    IEnumerator CheckTextForChar()
    {
        yield return new WaitWhile(() => _input.text.Length <= 0);

        foreach (var c in _input.text)
        {
            if (char.IsDigit(c) == false)
            {
                Debug.LogWarning("Character detected!");
                Debug.LogWarning("Please enter a number...");
                _input.text = "";
            }
        }

        _savedNum = _input.text;

        StartCoroutine(CheckTextForChar());
    }

    IEnumerator CheckTextLength()
    {
        // Level numbers can be 2 digits MAX
        yield return new WaitWhile(() => _input.text.Length <= _charLimit);
        _input.text = "";
        Debug.LogWarning("Char limit reached!");
        StartCoroutine(CheckTextLength());
    }

    private void HandleMouse()
    {
        OnPlayLevel?.Invoke();
    }

    private void ClearInput()
    {
        // I know there is a builtin string method to remove chars from a string
        // But using that here forces the user to backspace/click the input box to enter text again
        // This makes it to whereas they can just immediately provide new input
        _input.text = "";
    }

    /// <summary>
    /// Returns the number entered into the input box
    /// </summary>
    /// <returns></returns>
    public static int GetNum()
    {
        var numConv = _savedNum;

        //Parses the string for integers and returns an int accordingly
        return int.Parse(numConv, System.Globalization.NumberStyles.Integer);
    }
}

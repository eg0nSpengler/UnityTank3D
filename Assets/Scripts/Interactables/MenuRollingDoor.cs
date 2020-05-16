using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRollingDoor : MonoBehaviour
{

    [Header("References")]
    public AudioClip DoorOpenSound;
    public AudioClip DoorCloseSound;

    private AudioSource _audioSource;
    private GUIStyle _toolTipForeground;
    private GUIStyle _toolTipBackground;

    private const float _transAmount = 2.0f;
    private const string _toolTipText = "Start Game";
    private string _currentToolTipText;


    private void Awake()
    {

        _audioSource = GetComponent<AudioSource>();

        if (!_audioSource)
        {
            Debug.LogError("Failed to get Audio Source on " + gameObject.name.ToString() + ", creating one now...");
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        _toolTipForeground = new GUIStyle();
        _toolTipBackground = new GUIStyle();

        _toolTipForeground.normal.textColor = Color.white;
        _toolTipForeground.alignment = TextAnchor.UpperCenter;
        _toolTipForeground.wordWrap = true;

        _toolTipBackground.normal.textColor = Color.black;
        _toolTipBackground.alignment = TextAnchor.UpperCenter;
        _toolTipBackground.wordWrap = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        Debug.Log("Menu Door is being moused over!");
        _audioSource.clip = DoorOpenSound;
        _audioSource.Play();
        gameObject.transform.Translate(Vector3.up * _transAmount, Space.World);
        _currentToolTipText = _toolTipText;

    }

    private void OnMouseExit()
    {
        _audioSource.clip = DoorCloseSound;
        _audioSource.Play();
        gameObject.transform.Translate(Vector3.up * _transAmount * -1, Space.World);
        _currentToolTipText = " ";

    }

    private void OnMouseDown()
    {
        Debug.Log("MenuRollingDoor clicked on, at this point we'd load into the first level briefing scene");
        GameManager.InvokePreBriefingDelegate(gameObject);
    }

    private void OnGUI()
    {
        //And here I was thinking I'd have to do some camera to Worldspace conversion for the mouse position
        //Yet again, some obscure thread from almost a decade ago has the answer to your solution
        //Rather than some 1-3yr old threads posted on the most recent Unity version
        //Seriously, it's mind boggling
        if (_currentToolTipText != " ")
        {
            var x = Event.current.mousePosition.x;
            var y = Event.current.mousePosition.y;
            GUI.Label(new Rect(x - 149, y + 21, 300, 60), _currentToolTipText, _toolTipBackground);
            GUI.Label(new Rect(x - 150, y + 20, 300, 60), _currentToolTipText, _toolTipForeground);
        }
    }
}

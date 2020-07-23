using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupDisplayBox : MonoBehaviour
{
    [Header("References")]
    public Image pickupImage;


    public delegate void PickupScoreUpdate();
    public delegate void PickupScoreEnd();
    public delegate void PickupScoreGood();
    public delegate void PickupScoreBad();


    /// <summary>
    /// Called when the player score is updated from their pickup count
    /// </summary>
    public static event PickupScoreUpdate OnPickupScoreUpdate;

    /// <summary>
    /// Called when the player score has finished updated from their pickup count
    /// </summary>
    public static event PickupScoreEnd OnPickupScoreEnd;

    /// <summary>
    /// Called if the player manages to save atleast half or over half of the civilians in a level
    /// </summary>
    public static event PickupScoreGood OnPickupScoreGood;

    /// <summary>
    /// Called if the player saves only 1 civilian or less in a level
    /// </summary>
    public static event PickupScoreBad OnPickupScoreBad;

    private GameObject _parentPanel;
    private static List<Image> _imgList;

    private int _imgListIter;


    private void Awake()
    {
        _parentPanel = gameObject;

        _imgList = new List<Image>();

    }
    // Start is called before the first frame update
    void Start()
    {

        if (GameManager.GameState == GameManager.GAME_STATE.STATE_POSTBRIEFING)
        {
            DisplaySavedPickups();
            PickupScore();
        }
        else
        {
            PopulatePickupBox();
        }

    }

    private void OnEnable()
    {
        PickupManager.OnPickupCollected += UpdatePickups;
        GameManager.OnGameStatePostBrief += DisplaySavedPickups;
        Debug.Log("PickupDisplaybox created at " + Time.time.ToString());
    }

    private void OnDisable()
    {
        PickupManager.OnPickupCollected -= UpdatePickups;
        GameManager.OnGameStatePostBrief -= DisplaySavedPickups;
        Debug.Log("PickupDisplaybox destroyed at " + Time.time.ToString());
    }

    void PopulatePickupBox()
    {
        for (var i = 0; i < PickupManager.NumPickupsInLevel; i++)
        {
            var imgInstance = Instantiate(pickupImage, _parentPanel.transform);
            imgInstance.color = Color.white;
            _imgList.Add(imgInstance);
        }

        _imgListIter = 0;
    }

    void UpdatePickups()
    {
        if (PickupManager.LastPickupBool == true)
        {
            _imgList[_imgListIter].color = Color.green;
        }
        else
        {
            _imgList[_imgListIter].color = Color.red;
        }

        _imgListIter++;
    }

    void DisplaySavedPickups()
    {
        Debug.Log("DisplaySavedPickups called!");

        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);

        var totalPickups = gmData.numPickupsCollected + gmData.numPickupsLost;

        for (var i = 0; i < totalPickups; i++)
        {
           var imgInstance = Instantiate(pickupImage, _parentPanel.transform);                
           _imgList.Add(imgInstance);
        }

        _imgListIter = _imgList.Count - 1;

        foreach (var pk in gmData.pickupBool)
        {
            if (pk == true)
            {
                _imgList[_imgListIter].color = Color.green;
            }
            else
            {
                _imgList[_imgListIter].color = Color.red;
            }

            _imgListIter--;
        }
    }

    void PickupScore()
    {
        StartCoroutine(HandlePickupScore());
    }

    IEnumerator HandlePickupScore()
    {
        var civMultipler = 10000; // The amount of points to grant for each civvie saved
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);
        var currScore = gmData.playerScore;
        var totalPickups = gmData.numPickupsCollected + gmData.numPickupsLost;

        //This is just to pre-calculate the score so we can decide
        //If we should display the "Savior bonus" text
        var pScore = gmData.playerScore;

        foreach (var img in _imgList)
        {
            if (img.color == Color.green)
            {
                pScore += civMultipler;    
            }
        }

        var half = totalPickups / 2;

        // The player only saved a whopping 1 civilian or less
        if (pScore == currScore + civMultipler || pScore == currScore)
        {
            OnPickupScoreBad?.Invoke();
        }

        // The player saves >= half of the civilians in a level
        if (pScore >= currScore + (civMultipler * half))
        {
            OnPickupScoreGood?.Invoke();
        }

        // Here we calculate for the actual score that will be serialized
        foreach (var img in _imgList)
        {
            // Very simple
            // If the image is green, we know it's a civvie we collected
            if (img.color == Color.green)
            {
                yield return new WaitForSeconds(0.40f);
                img.color = Color.white;
                gmData.playerScore += civMultipler;
                OnPickupScoreUpdate?.Invoke();
            }

        }

        OnPickupScoreEnd?.Invoke();

        // I call the coroutine in the scope of this class
        // Because for some odd reason the coroutine becomes null or simply doesn't call at all
        // When I call it from within the scope of the LevelTimerBox class itself
        // Mind boggling because there are other MonoBehaviours in this game
        // That call their coroutines just fine in response to raised events
        // I've tried nearly everything I can think of
        // This is something I'd rather not do, but for now this shall suffice
        StartCoroutine(LevelTimerBox.HandleTimeScore());

    }

}

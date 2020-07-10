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

        PickupManager.OnPickupCollected += UpdatePickups;
        GameManager.OnGameStatePostBrief += DisplaySavedPickups;

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

    private void OnDisable()
    {
        PickupManager.OnPickupCollected -= UpdatePickups;
        GameManager.OnGameStatePostBrief -= DisplaySavedPickups;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulatePickupBox()
    {
        for (var i = 0; i < PickupManager.NumPickupsInLevel; i++)
        {
            var imgInstance = Instantiate(pickupImage, _parentPanel.transform);
            imgInstance.color = Color.white;
            _imgList.Add(imgInstance);
        }

        _imgListIter = _imgList.Count - 1;
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

        _imgListIter--;
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
        var civMultipler = 10000;
        var end = GameDataSerializer._gameDataList.Count - 1;
        var gmData = GameDataSerializer.LoadGameData(end);
        var currScore = gmData.playerScore;
        var totalPickups = gmData.numPickupsCollected + gmData.numPickupsLost;

        foreach (var img in _imgList)
        {
            if (img.color == Color.green)
            {
                yield return new WaitForSeconds(0.40f);
                img.color = Color.white;
                gmData.playerScore += civMultipler;
                OnPickupScoreUpdate?.Invoke();
            }

        }

        var half = totalPickups / 2;

        // The player only saved a whopping 1 civilian or less
        if (gmData.playerScore == currScore + civMultipler || gmData.playerScore == currScore)
        {
            OnPickupScoreBad?.Invoke();
        }

        // The player saves >= half of the civilians in a level
        if (gmData.playerScore >= currScore + (civMultipler * half))
        {
            OnPickupScoreGood?.Invoke();
        }

        OnPickupScoreEnd?.Invoke();
    }

}

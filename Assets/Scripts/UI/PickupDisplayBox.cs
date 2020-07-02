using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupDisplayBox : MonoBehaviour
{
    [Header("References")]
    public Image pickupImage;

    private GameObject _parentPanel;
    private static List<Image> _imgList;

    private int _imgListIter;

    private void Awake()
    {
        _parentPanel = gameObject;

        _imgList = new List<Image>();

        PickupManager.OnPickupCollected += UpdatePickups;
        LevelPortal.OnLevelPortalEnabled += SavePickups;
        GameManager.OnGameStatePostBrief += DisplaySavedPickups;

    }
    // Start is called before the first frame update
    void Start()
    {

        for (var i = 0; i < PickupManager.NumPickupsInLevel; i++)
        {
            Image imgInstance = Instantiate(pickupImage, _parentPanel.transform);
            imgInstance.color = Color.white;
            _imgList.Add(imgInstance);
        }

        _imgListIter = _imgList.Count - 1;
        
    }

    private void OnDisable()
    {
        PickupManager.OnPickupCollected -= UpdatePickups;
        LevelPortal.OnLevelPortalEnabled -= SavePickups;
        GameManager.OnGameStatePostBrief -= DisplaySavedPickups;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    void SavePickups()
    {
        
    }

    void DisplaySavedPickups()
    {
        foreach (var img in _imgList)
        {
            foreach (var result in PickupManager.GetPickupBoolList())
            {
                if (result == true)
                {
                    _imgList[_imgListIter].color = Color.green;
                }
                else
                {
                    _imgList[_imgListIter].color = Color.red;
                }
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupDisplayBox : MonoBehaviour
{
    [Header("References")]
    public Image pickupImage;

    private int imgListIter;
    private GameObject _parentPanel;
    private static List<Image> _imgList;

    private void Awake()
    {
        _parentPanel = gameObject;

        _imgList = new List<Image>();

        PickupManager.OnPickupCollected += UpdatePickups;

    }
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < PickupManager.GetNumPickupsInLevel(); i++)
        {
            Image imgInstance = Instantiate(pickupImage, _parentPanel.transform);
            imgInstance.color = Color.white;
            _imgList.Add(imgInstance);
        }

        imgListIter = _imgList.Count - 1;

        
    }

    private void OnDisable()
    {
        PickupManager.OnPickupCollected -= UpdatePickups;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePickups()
    {
        _imgList[imgListIter].color = Color.green;
        imgListIter--;
    }
}

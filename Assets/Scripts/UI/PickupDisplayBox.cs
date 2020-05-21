using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupDisplayBox : MonoBehaviour
{

    public Image pickupImage;

    private GameObject _parentPanel;
    private List<Image> _pickupimages;


    private void Awake()
    {
        _parentPanel = gameObject;
        _pickupimages = new List<Image>();

        if (!pickupImage)
        {
            Debug.LogError("No pickup image set for PickupDisplayBox!");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _pickupimages.Add(pickupImage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

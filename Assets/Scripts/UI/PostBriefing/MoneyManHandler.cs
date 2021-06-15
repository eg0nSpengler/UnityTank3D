using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MoneyManHandler : MonoBehaviour
{
    [Header("Sprite References")]
    public Sprite MoneyManPissed;

    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();

        if (!MoneyManPissed)
        {
            Debug.LogWarning(gameObject.name.ToString() + " is missing an image reference!");
        }

    }

    private void OnEnable()
    {
        PickupDisplayBox.OnPickupScoreBad += ShowPissedMan;
    }

    private void OnDisable()
    {
        PickupDisplayBox.OnPickupScoreBad -= ShowPissedMan;
    }

    void ShowPissedMan()
    {
        _img.sprite = MoneyManPissed;
    }
}

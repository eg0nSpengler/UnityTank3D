﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TankHealthBox : MonoBehaviour
{
    public TankActor _tankActor;

    [Header("Tank Sprite References")]
    public Sprite TankHealthFull;
    public Sprite TankHealthHalf;
    public Sprite TankHealthLow;

    private HealthComponent _healthComp;
    private Image _panel;

    private void Awake()
    {
        _panel = GetComponent<Image>();
        _tankActor = FindObjectOfType<TankActor>();
        _healthComp = _tankActor.GetComponent<HealthComponent>();

        if (!_panel)
        {
            Debug.LogError("Failed to get Image on " + gameObject.name.ToString() + ", creating one now...");
            _panel = gameObject.AddComponent<Image>();
        }

        if (!_tankActor)
        {
            Debug.LogError("Failed to find TankActor reference in TankHealthBox!");
            Debug.LogError("This is deliberate if you are currently within the POST-BRIEFING scene");
        }

        if (!TankHealthFull || !TankHealthHalf || !TankHealthLow)
        {
            Debug.LogWarning("TankHealthBox is missing a Sprite reference!");
        }
   

    }

    // Start is called before the first frame update
    void Start()
    {
        _panel.sprite = TankHealthFull;
    }

    private void OnEnable()
    {
        _healthComp.OnHealthModified += UpdateHealth;
    }

    private void OnDisable()
    {
        _healthComp.OnHealthModified -= UpdateHealth;
    }


    void UpdateHealth()
    {
        if (!_tankActor)
        {
            _panel.sprite = TankHealthFull;
            Debug.LogWarning("Panel Sprite is currently set to TankHealthFull in TankHealthBox");
            Debug.LogError("This is deliberate if you are currently within the POST-BRIEFING scene");
        }

        var hp = _healthComp.CurrentHP;

        if (hp >= _healthComp.MaxHP)
        {
            _panel.sprite = TankHealthFull;
        }

        if (hp < _healthComp.MaxHP)
        {
            if (hp == 20)
            {
                _panel.sprite = TankHealthHalf;
            }
            else
            {
                _panel.sprite = TankHealthLow;
            }
        }
    }
}

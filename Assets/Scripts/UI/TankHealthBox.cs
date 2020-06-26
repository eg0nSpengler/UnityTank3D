using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealthBox : MonoBehaviour
{
    public TankActor _tankActor;

    private HealthComponent _healthComp;
    private Image _panel;

    private float _maxFillValue;
    private float _minFillValue;
    //The amount to add to the mask fill
    private float _fillAddValue;

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
        }

        _maxFillValue = 1.0f;
        _minFillValue = 0.0f;
        _fillAddValue = 0.10f;

        _panel.fillAmount = _minFillValue;

        _healthComp.OnHealthModified += UpdateHealth;

    }

    // Start is called before the first frame update
    void Start()
    {
        _panel.fillAmount = 1;
    }

    private void OnDisable()
    {
        _healthComp.OnHealthModified -= UpdateHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth()
    {
        var hp = _healthComp.GetHealth();

        if (hp >= _healthComp.GetMaxHealth())
        {
            _panel.fillAmount = _minFillValue;
        }

        if (hp < _healthComp.GetMaxHealth())
        {
            var amt = hp *- _fillAddValue;
            Debug.Log(amt.ToString());

            _panel.fillAmount = _panel.fillAmount + amt;
        }
    }
}

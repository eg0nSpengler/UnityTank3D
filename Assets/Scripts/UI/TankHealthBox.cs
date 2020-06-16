using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealthBox : MonoBehaviour
{
    private TankActor _tankActor;
    private HealthComponent _healthComp;
    private Image _panel;

    private float maxFillValue = 1.0f;
    private float minFillValue = 0.0f;
    //The amount to add to the mask fill
    private float fillAddValue = 0.10f;

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

        _panel.fillAmount = minFillValue;
        _healthComp.OnHealthModified += UpdateHealth;

    }

    // Start is called before the first frame update
    void Start()
    {
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
            _panel.fillAmount = minFillValue;
        }

        if (hp < _healthComp.GetMaxHealth())
        {
            var amt = hp * fillAddValue;

            _panel.fillAmount = _panel.fillAmount + amt;
        }
    }
}

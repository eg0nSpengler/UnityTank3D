using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealthBox : MonoBehaviour
{
    private Image _panel;

    public HealthComponent healthComponent;

    private float maxFillValue = 1.0f;
    private float minFillValue = 0.0f;
    //The amount to add to the mask fill
    private float fillAddValue = 0.10f;

    private void Awake()
    {
        _panel = GetComponent<Image>();

        if (!_panel)
        {
            Debug.LogError("Failed to get Image on " + gameObject.name.ToString() + ", creating one now...");
            _panel = gameObject.AddComponent<Image>();
        }

        _panel.fillAmount = minFillValue;

        HealthComponent.OnHealthModified += UpdateHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealth()
    {
        var hp = healthComponent.GetHealth();

        if (hp >= 10)
        {
            _panel.fillAmount = minFillValue;
        }

        if (hp < 10)
        {
            var amt = hp * fillAddValue;

            _panel.fillAmount = _panel.fillAmount + amt;
        }
    }
}

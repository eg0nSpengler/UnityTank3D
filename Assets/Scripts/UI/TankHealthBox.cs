using System.Collections;
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
        }

        if (!TankHealthFull || !TankHealthHalf || !TankHealthLow)
        {
            Debug.LogWarning("TankHealthBox is missing a Sprite reference!");
        }

        _healthComp.OnHealthModified += UpdateHealth;

    }

    // Start is called before the first frame update
    void Start()
    {
        _panel.sprite = TankHealthFull;
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

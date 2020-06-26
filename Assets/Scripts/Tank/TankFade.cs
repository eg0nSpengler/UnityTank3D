using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is for handling the fade to white when the Player dies
/// </summary>
public class TankFade : MonoBehaviour
{
    [Header("References")]
    public Sprite deathImage;

    private HealthComponent _healthComp;
    private Image _img;

    private void Awake()
    {
        _healthComp = GetComponentInParent<HealthComponent>();
        _img = GetComponent<Image>();

        if (!_healthComp)
        {
            Debug.LogError("Failed to get HealthComponent on TankFade!");
        }

        if (!_img)
        {
            Debug.LogError("Failed to get Image on TankFade, creating one now.");
            _img = gameObject.AddComponent<Image>();
        }

        if(!deathImage)
        {
            Debug.LogWarning("No DeathImage set on TankFade!");
        }

        _healthComp.OnHealthZero += DoDeathFade;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoDeathFade()
    {
        StartCoroutine(DeathFade());
    }

    IEnumerator DeathFade()
    {
        yield return new WaitForSeconds(1f);

        for (var ft = 0f; ft <= 1.5; ft += 0.1f)
        {
            _img.color = new Color(Color.white.r, Color.white.g, Color.white.b, ft);
            yield return null;
        }

        _img.sprite = deathImage;
    }
}

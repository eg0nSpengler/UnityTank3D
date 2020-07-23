using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunStatusBox : MonoBehaviour
{
    [Header("Sprite References")]
    public Sprite GunRearming;
    public Sprite GunReady;
    public Sprite GunCharging;
    public Sprite GunMaxPower;

    public TankGun _tankGun;

    private Image _panel;

    private void Awake()
    {
        _tankGun = FindObjectOfType<TankGun>();
        _panel = GetComponent<Image>();

        if (!_tankGun)
        {
            Debug.LogError("No TankGun found in the current scene!");
            Debug.LogError("You may have forgotten to place a TankActor instance or you may be within the POST-BRIEFING scene!");
        }

        if (!GunRearming || !GunReady || !GunCharging || !GunMaxPower)
        {
            Debug.LogWarning("GunStatusBox is missing a Sprite reference!");
        }

        if (!_panel)
        {
            Debug.LogError("Failed to get Panel on GunStatusBox, creating one now");
            _panel = gameObject.AddComponent<Image>();
        }

        _panel.preserveAspect = true;

    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGunStatusText();
    }


    private void OnEnable()
    {
        TankGun.OnGunStatusUpdate += UpdateGunStatusText;
        GameManager.OnGameStatePostBrief += ShowDefaultGunStatus;
    }

    private void OnDisable()
    {
        TankGun.OnGunStatusUpdate -= UpdateGunStatusText;
        GameManager.OnGameStatePostBrief -= ShowDefaultGunStatus;
    }

    void UpdateGunStatusText()
    {
        // This is done for the POST-BRIEFING scene
        // Only since we don't place a TankActor instance in the POST briefing scenes
        if (!_tankGun)
        {
            _panel.sprite = GunReady;
            Debug.LogWarning("GunStatus is currently GunReady due to no TankActor instance being found in the current scene");
            Debug.LogWarning("This is deliberate if you are currently within the POST-BRIEFING scene");
        }
        else
        {
            switch (_tankGun.GunStatusToString())
            {
                case "GUN READY":
                    _panel.sprite = GunReady;
                    break;
                case "CHARGING":
                    _panel.sprite = GunCharging;
                    break;

                case "REARMING":
                    _panel.sprite = GunRearming;
                    break;

                case "MAX CHARGE":
                    _panel.sprite = GunMaxPower;
                    break;

                default: Debug.LogWarning("UpdateGunStatus failed to get valid GunState");
                    Debug.LogWarning("Double check the string comparisons");
                    _panel.sprite = GunReady;
                    break;
            }
            
        }
    }

    void ShowDefaultGunStatus()
    {
        _panel.sprite = GunReady;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunStatusBox : MonoBehaviour
{
    public TankGun _tankGun;

    private TextMeshProUGUI _gunStatus;

    private void Awake()
    {
        _gunStatus = GetComponent<TextMeshProUGUI>();
        _tankGun = FindObjectOfType<TankGun>();

        if (!_gunStatus)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _gunStatus = gameObject.AddComponent<TextMeshProUGUI>();
        }

        if(!_tankGun)
        {
            Debug.LogError("No TankGun found in the current scene!");
            Debug.LogError("You may have forgotten to place a TankActor instance!");
        }

        TankGun.OnGunStatusUpdate += UpdateGunStatusText;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGunStatusText();
    }

    private void OnDisable()
    {
        TankGun.OnGunStatusUpdate -= UpdateGunStatusText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateGunStatusText()
    {
        _gunStatus.text = _tankGun.GunStatusToString();
        switch (_gunStatus.text.ToString())
        {

            case "GUN READY":
                _gunStatus.color = Color.green;
                break;

            case "REARMING":
                _gunStatus.color = Color.red;
                break;

            case "CHARGING":
                _gunStatus.color = Color.cyan;
                break;

            case "MAX CHARGE":
                _gunStatus.color = Color.magenta;
                break;

            default:
                _gunStatus.color = Color.green;
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunStatusBox : MonoBehaviour
{
    private TextMeshProUGUI _gunStatus;

    private void Awake()
    {
        _gunStatus = GetComponent<TextMeshProUGUI>();

        if (!_gunStatus)
        {
            Debug.LogError("Failed to get TextMeshProTextUI element on " + gameObject.name.ToString() + ", creating one now...");
            _gunStatus = gameObject.AddComponent<TextMeshProUGUI>();
        }

        TankGun.OnGunStatusUpdate += UpdateGunStatusText;
    }
    // Start is called before the first frame update
    void Start()
    {
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
        _gunStatus.text = TankGun.GunStatusToString();
        switch (_gunStatus.text.ToString())
        {
            case "REARMING":
                _gunStatus.color = Color.red;
                break;

            default:
                _gunStatus.color = Color.green;
                break;
        }
    }
}

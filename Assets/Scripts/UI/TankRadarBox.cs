using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankRadarBox : MonoBehaviour
{

    [Header("References")]
    public Image FriendlyRadarBlip;
    public Transform playerPosition;
    public SphereCollider playerRadarSphere;

    /// <summary>
    /// A List of all the Pickup locations in the currently loaded level
    /// </summary>
    private List<Vector3> _pickupPositions;

    private List<Image> _imgList;

    public RectTransform _parentPanel;

    private void Awake()
    {
        _parentPanel = gameObject.GetComponent<RectTransform>();
        _pickupPositions = new List<Vector3>();
        _imgList = new List<Image>();

        if (!FriendlyRadarBlip)
        {
            Debug.LogError("No FriendlyRadarBlip image set in TankRadarBox!");
            Debug.LogError("You probably forgot to assign one in the Inspector");
        }

        if (!playerPosition)
        {
            Debug.LogError("No PlayerPosition set in TankRadarBox!");
            Debug.LogError("You probably forgot to assign it in the Inspector");
        }

        if (!playerRadarSphere)
        {
            Debug.LogError("No PlayerRadarSphere set in TankRadarBox!");
            Debug.LogError("You probably forgot to assign it in the Inspector");
        }

        TankRadar.OnPickupInRange += DrawRadarBlips;
        SphereHandler.OnPickupCollectedEvent += UpdateRadarBlips;
    }

    // Start is called before the first frame update
    void Start()
    { 

        foreach (var pos in PickupManager.GetPickupPositions())
        {
            _pickupPositions.Add(pos);
        }

        CreateBlips();

    }

    private void OnDisable()
    {
        TankRadar.OnPickupInRange -= DrawRadarBlips;
        SphereHandler.OnPickupCollectedEvent -= UpdateRadarBlips;
    }

    // Update is called once per frame
    void Update()
    {
        DrawRadarBlips();
    }

    void CreateBlips()
    {
        foreach (var pos in _pickupPositions)
        {
            Image imgInstance = Instantiate(FriendlyRadarBlip, _parentPanel.transform);
            imgInstance.rectTransform.anchoredPosition = _parentPanel.rect.center;
            imgInstance.color = Color.white;
            _imgList.Add(imgInstance);
        }
    }

    void DrawRadarBlips()
    {
        for (var x = 0; x < _imgList.Count; x++)
        {
            for(var i = 0; i < _pickupPositions.Count; i++)
            {
                // I honestly never want to do a hack like this again
                // Okay so it's not really a hack exactly but...
                // I think I just about pulled my hair out on this bit trying to figure this out
                // CONVERTING 3D WORLDSPACE POSITIONS TO 2D LOCAL SPACE POSITIONS HAHAHAH SO MUCH FUN
                // It works, that's all that matters.

                var currentPos = _pickupPositions[i];
                var radarPos = currentPos - playerPosition.position;
                var dist = Vector3.Distance(playerPosition.position, currentPos);
                var deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - playerPosition.eulerAngles.y;
                radarPos.x = dist * Mathf.Cos(deltay * Mathf.Deg2Rad) * -1;
                radarPos.z = dist * Mathf.Sin(deltay * Mathf.Deg2Rad);
                var radarVec = new Vector3(radarPos.x, radarPos.z, 0.0f);
                _imgList[x].rectTransform.localPosition = radarVec;

                if (x == i)
                {
                    break;
                }
            }        
            
        }
    }

    void UpdateRadarBlips()
    {
        
    }

}


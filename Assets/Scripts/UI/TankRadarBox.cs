using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankRadarBox : MonoBehaviour
{

    [Header("References")]
    public Image RadarBlip;
    public Transform playerPosition;
    public SphereCollider playerRadarSphere;

    private LevelPortal _levelPortal;

    /// <summary>
    /// A List of all the Pickup locations in the currently loaded level
    /// </summary>
    private List<Vector3> _pickupPositions;

    /// <summary>
    /// A List of radar blips to draw on the radar panel
    /// White Blip - Pickup
    /// Red Blip - Enemy
    /// Yellow Blip - Level Portal
    /// </summary>
    private List<Image> _imgList;

    /// <summary>
    /// The parent radar panel
    /// </summary>
    private RectTransform _parentPanel;

    private void Awake()
    {
        _parentPanel = gameObject.GetComponent<RectTransform>();
        _pickupPositions = new List<Vector3>();
        _imgList = new List<Image>();
        _levelPortal = FindObjectOfType<LevelPortal>();

        if (!RadarBlip)
        {
            Debug.LogError("No RadarBlip image set in TankRadarBox!");
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

        if (_levelPortal == null)
        {
            Debug.LogError("No PortalActor found in the current scene!");
            Debug.LogError("You may have forgotten to place a PortalActor instance!");
        }

        TankRadar.OnPickupInRange += DrawRadarBlips;
        PickupManager.OnPickupCollected += UpdateRadarBlips;
        LevelPortal.OnLevelPortalEnabled += ShowPortalRadarBlip;
    }

    // Start is called before the first frame update
    void Start()
    { 
        // For the PickupPosition List, we add all the pickup positions in the scene to the list

        foreach (var pos in PickupManager.GetPickupPositions())
        {
            _pickupPositions.Add(pos);
        }

        // We reserve the last element in the list for the LevelPortal position
        _pickupPositions.Add(_levelPortal.transform.position);

        CreateBlips();

    }

    private void OnDisable()
    {
        TankRadar.OnPickupInRange -= DrawRadarBlips;
        PickupManager.OnPickupCollected -= UpdateRadarBlips;
        LevelPortal.OnLevelPortalEnabled -= ShowPortalRadarBlip;
    }

    // Update is called once per frame
    void Update()
    {
        DrawRadarBlips();
    }

    /// <summary>
    /// Creates the radar "contacts" and adds them to the radar panel
    /// </summary>
    void CreateBlips()
    {
        //Minus one since the last element is the LevelPortal Position
        for (var i = 0; i < _pickupPositions.Count - 1; i++)
        {
            Image imgInstance = Instantiate(RadarBlip, _parentPanel.transform);
            imgInstance.rectTransform.anchoredPosition = _parentPanel.rect.center;
            imgInstance.color = Color.white;
            _imgList.Add(imgInstance);
        }

        // While it isn't a pickup
        // We add the LevelPortal pos to our radar so that we can render it when the portal appears
        // We simply set the blip color to Clear so that it doesn't show on the radar panel until the actual LevelPortal gameobject is enabled in scene
        Image lvlPortal = Instantiate(RadarBlip, _parentPanel.transform);
        lvlPortal.rectTransform.anchoredPosition = _parentPanel.rect.center;
        lvlPortal.color = Color.clear;
        _imgList.Add(lvlPortal);

    }

    /// <summary>
    /// Draws each radar blip on the radar panel
    /// </summary>
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

                // Just so we don't apply the same panel position to each radar blip
                if (x == i)
                {
                    break;
                }
            }        
        }
    }

    void UpdateRadarBlips()
    {
            // Looping through all of our pickup positions
            for (var y = 0; y < _pickupPositions.Count; y++)
            {
                // If a position in our list matches the position of a recently collected pickup, "remove" it from the radar
                if (_pickupPositions[y] == PickupManager.GetRecentCollectedPos())
                {
                    // I'd rather not explicitly delete the Image instance, so we just set the color to transparent
                    // The Pickup Positions List and the Image List both share the same indexes (Element 0 in the pos List corresponds directly Element 0 in the img List and it's associated radar blip)
                    // Hence why we can just use this same iterator for accessing the Image List
                    _imgList[y].color = Color.clear;
                    return;
                }
            }
    }

    void ShowPortalRadarBlip()
    {
        //This sets the LevelPortal radar blip color to yellow, making it finally *appear* on radar.
        _imgList[_imgList.Count - 1].color = Color.yellow;
        // *gasp*
        // IS THIS A DIRECT CONTAINER ELEMENT ACCESS **WITHOUT** BOUNDS CHECKING????
        // *gasp*
        // :)
    }
}


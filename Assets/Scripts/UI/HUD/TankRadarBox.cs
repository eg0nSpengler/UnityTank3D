using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankRadarBox : MonoBehaviour
{

    [Header("References")]
    public Image RadarBlip;
    //public SphereCollider playerRadarSphere;

    /// <summary>
    /// The player's position in the level
    /// </summary>
    private Transform _playerPosition;

    /// <summary>
    /// A List of all the Pickup locations in the currently loaded level
    /// </summary>
    private List<Vector3> _pickupPositions;

    /// <summary>
    /// A List of radar blips to draw on the radar panel
    /// </summary>
    private List<Image> _imgList;

    /// <summary>
    /// The parent radar panel
    /// </summary>
    private RectTransform _parentPanel;

    private LevelPortal _levelPortal;

    /// <summary>
    /// A ptr offset to the last pickup pos element in the pickupPos container
    /// </summary>
    private int _pickupOffSet;

    /// <summary>
    /// A ptr offset to the last mob pos element in the pickupPos container
    /// </summary>
    private int _mobOffSet; 

    private void Awake()
    {
        _parentPanel = gameObject.GetComponent<RectTransform>();
        _pickupPositions = new List<Vector3>();
        _imgList = new List<Image>();
        _pickupOffSet = 0;
        _mobOffSet = 0;
        _levelPortal = FindObjectOfType<LevelPortal>();

        //Getting the Player transform
        foreach (var transForm in FindObjectsOfType<Transform>())
        {
            if (transForm.gameObject.name == TagStatics.GetPlayerName())
            {
                _playerPosition = transForm;
            }
        }

        if (!RadarBlip)
        {
            Debug.LogError("No RadarBlip image set in TankRadarBox!");
            Debug.LogError("You probably forgot to assign one in the Inspector");
        }

        if (!_playerPosition)
        {
            Debug.LogError("No PlayerPosition set in TankRadarBox!");
        }

        /*if (!playerRadarSphere)
        {
            Debug.LogError("No PlayerRadarSphere set in TankRadarBox!");
            Debug.LogError("You probably forgot to assign it in the Inspector");
        }*/

        if (_levelPortal == null)
        {
            Debug.LogError("No PortalActor found in the current scene!");
            Debug.LogError("You may have forgotten to place a PortalActor instance!");
        }

    }

    // Start is called before the first frame update
    void Start()
    { 
        // For the PickupPosition List, we add all the pickup positions in the scene to the list
        foreach (var pos in PickupManager.GetPickupPositions())
        {
            _pickupPositions.Add(pos);
        }

        //Setting our pickupOffset to point to the last pickup position in the list
        _pickupOffSet = _pickupPositions.Count - 1;

        // We then add all the current MobPositions to the list
        foreach (var pos in MobManager.GetMobPositions())
        {
            _pickupPositions.Add(pos);
        }

        //Like before except here, we're setting it to the last mob position in the list
        _mobOffSet = _pickupPositions.Count - 1;

        // We reserve the last element in the list for the LevelPortal position
        _pickupPositions.Add(_levelPortal.transform.position);

        CreateBlips();

    }

    private void OnEnable()
    {
        TankRadar.OnPickupInRange += DrawRadarBlips;
        PickupManager.OnPickupCollected += RemoveRadarBlip;
        MobManager.OnMobDestroyed += RemoveRadarBlip;
        if (FindObjectOfType<LevelPortal>() != null)
        {
            FindObjectOfType<LevelPortal>().OnLevelPortalEnabled += ShowPortalRadarBlip;
        }
    }

    private void OnDisable()
    {
        TankRadar.OnPickupInRange -= DrawRadarBlips;
        PickupManager.OnPickupCollected -= RemoveRadarBlip;
        MobManager.OnMobDestroyed -= RemoveRadarBlip;
        if (FindObjectOfType<LevelPortal>() != null)
        {
          FindObjectOfType<LevelPortal>().OnLevelPortalEnabled -= ShowPortalRadarBlip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRadarBlips();
        DrawRadarBlips();
    }


    /// <summary>
    /// Creates the radar "contacts" and adds them to the radar panel
    /// </summary>
    void CreateBlips()
    {
        /// White Blip - Pickup
        /// Red Blip - Enemy
        /// Yellow Blip - Level Portal
       
        //Minus one since the last element is the LevelPortal Position
        for (var i = 0; i < _pickupPositions.Count - 1; i++)
        {
            Image imgInstance = Instantiate(RadarBlip, _parentPanel.transform);
            imgInstance.rectTransform.anchoredPosition = _parentPanel.rect.center;
            _imgList.Add(imgInstance);
        }

        // Going backwards
        // From the last pickup pos element to the first
        for (var x = _pickupOffSet; x >= 0; x--)
        {
            _imgList[x].color = Color.white;
        }

        // Again going backwards
        // From the last mob pos element to the first
        for (var y = _mobOffSet; y > _pickupOffSet; y--)
        {
            _imgList[y].color = Color.red;
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
                var distBetween = Vector3.Distance(_imgList[x].transform.position, _parentPanel.transform.position);

                var currentPos = _pickupPositions[i];
                var radarPos = currentPos - _playerPosition.position;
                var dist = Vector3.Distance(_playerPosition.position, currentPos);
                var deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - _playerPosition.eulerAngles.y;
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

    void RemoveRadarBlip()
    {
        var pickupPos = PickupManager.LastCollectedPos;
        var mobPos = MobManager.LastDestroyedPos;

        // Looping through all of our pickup positions
        for (var y = 0; y < _pickupPositions.Count; y++)
        {
            //If a position in our list matches the position of a recently destroyed mob, "remove" it from the radar
            if (_pickupPositions[y] == mobPos)
            {
                _imgList[y].color = Color.clear;
            }

            // If a position in our list matches the position of a recently collected pickup, "remove" it from the radar
            if (_pickupPositions[y] == pickupPos)
            {
                // I'd rather not explicitly delete the Image instance, so we just set the color to transparent
                // The Pickup Positions List and the Image List both share the same indexes (Element 0 in the pos List corresponds directly Element 0 in the img List and it's associated radar blip)
                // Hence why we can just use this same iterator for accessing the Image List
                _imgList[y].color = Color.clear;
            }
        }
    }

    /// <summary>
    /// This is for updating the radar blips for moving targets (Monsters, for example)
    /// </summary>
    
    void UpdateRadarBlips()
    {
        var newMobPos = new List<Vector3>();

        newMobPos.AddRange(MobManager.GetMobPositions());

        //TODO - Fix monster blips past mob elem 0 not updating
        for (var x = 0; x < newMobPos.Count; x++)
        {
            //Using our mob offset again
            for (var i = _mobOffSet; i > _pickupOffSet; i--)
            {
                _pickupPositions[i] = newMobPos[x];

                if (i != x)
                {
                    break;
                }
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


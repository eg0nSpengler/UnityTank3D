using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectionSphere : MonoBehaviour
{

    [Header("References")]
    public GameObject currentTarget;
    public Collider audioTargetCollider;

    private SphereCollider _sphereCollider;

    private float _sphereRadius = 5.0f;

    public delegate void TargetClearLOS(GameObject obj);
    public delegate void TargetNoClearLOS(GameObject obj);
    public delegate void TargetInRadius(GameObject obj);
    public delegate void TargetExitRadius(GameObject obj);
    public delegate void TargetTracking(GameObject obj);

    public delegate void HeardSound(GameObject obj);

    /// <summary>
    /// Called when we have a clear Line of Sight to a target
    /// </summary>
    public event TargetClearLOS OnTargetClearLOS;

    /// <summary>
    /// Called when we don't have a clear Line of Sight to a target
    /// </summary>
    public event TargetNoClearLOS OnTargetNoClearLOS;

    /// <summary>
    /// Called when a target has come inside the radius of our sphere
    /// </summary>
    public event TargetInRadius OnTargetInRadius;

    /// <summary>
    /// Called when a target exits the radius of our sphere
    /// </summary>
    public event TargetExitRadius OnTargetExitRadius;

    /// <summary>
    /// Called when we have a clear Line of Sight to a target and have begun tracking them
    /// </summary>
    public event TargetTracking OnTargetTracking;


    /// <summary>
    /// Called when we detect a noise
    /// </summary>
    public event HeardSound OnHeardSound;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();

        _sphereCollider.radius = _sphereRadius;
        _sphereCollider.isTrigger = true;

        if (!_sphereCollider)
        {
            Debug.LogError("Failed to get SphereCollider on " + gameObject.name.ToString() + ", creating one now...");
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
        }


        OnTargetTracking += SearchForTarget;
    }

    // Start is called before the first frame update
    private void Start()
    {
        OnTargetTracking -= SearchForTarget;
    }

    private void OnDisable()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        SearchForTarget(currentTarget);
    }

    private void OnTriggerEnter(Collider other)
    {

        // We detect a pickup in our radius
        if (other.gameObject.tag == TagStatics.GetPickupTag())
        {
            if (InLineOfSight(other.gameObject))
            {
                OnTargetClearLOS(other.gameObject);
                OnTargetInRadius(other.gameObject);
            }
            else
            {
                OnTargetNoClearLOS(other.gameObject);
                OnTargetInRadius(other.gameObject);
            }
        }

        if (other.gameObject.tag == TagStatics.GetMobTag() && other.gameObject.name == TagStatics.GetPlayerName())
        {
            //The mob is within our LoS
            if (InLineOfSight(other.gameObject))
            {
                OnTargetClearLOS(other.gameObject);
                OnTargetInRadius(other.gameObject);
            }
            else
            {
                OnTargetInRadius(other.gameObject);
                OnTargetNoClearLOS(other.gameObject);
                return;
            }
            currentTarget = other.gameObject;
        }


        //We heard a noise
        if (other.gameObject.tag == "Audible")
        {
            OnHeardSound(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TagStatics.GetMobTag() && other.gameObject.name == TagStatics.GetPlayerName())
        {
            OnTargetExitRadius(other.gameObject);
            currentTarget = null;
        }
    }

    /// <summary>
    /// This is to check if we have an unobstructed LoS to whoever enters the monster's search radius
    /// If it's unobstructed, we know that we can "see" them
    /// </summary>
    private bool InLineOfSight(GameObject obj)
    {
        var startPos = gameObject.transform.position;
        var endPos = (obj.transform.position - gameObject.transform.position);
        RaycastHit hit;

        if (Physics.Raycast(startPos, endPos, out hit) == true)
        {
            if (hit.collider.gameObject.name == obj.name)
            {
                OnTargetClearLOS(obj);
                OnTargetInRadius(obj);
                Debug.DrawRay(startPos, hit.point - startPos, Color.green);
                return true;
            }
            else
            {
                OnTargetNoClearLOS(obj);
                OnTargetInRadius(obj);
                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// This is to search for targets within our radius
    /// </summary>
    void SearchForTarget(GameObject obj)
    {
        //We have a reference to someone inside of our radius
        if (currentTarget)
        {

            //Checking to see if they are actually within our radius
            if (_sphereCollider.bounds.Contains(currentTarget.GetComponent<Collider>().bounds.center))
            {
                OnTargetClearLOS(currentTarget);
                OnTargetTracking(currentTarget);
            }
        }
    }
    
}

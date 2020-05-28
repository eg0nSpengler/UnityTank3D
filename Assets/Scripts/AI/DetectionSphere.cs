using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectionSphere : MonoBehaviour
{
 
    [Header("References")]
    public GameObject currentTarget;
    public Collider currentTargetCollider;
    public Collider audioTargetCollider;

    private Vector3 _lastKnownPos;
    private SphereCollider _sphereCollider;
    private NavMeshAgent _navAgent;
    private NavMeshPath _navMeshPath;

    private float _pathElapsed = 0.0f; //For NavMeshPath debugging purposes
    private float _sphereRadius = 5.0f;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();

        if (!_sphereCollider)
        {
            Debug.LogError("Failed to get SphereCollider on " + gameObject.name.ToString() + ", creating one now...");
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
        }

        _navAgent = GetComponentInParent<NavMeshAgent>();

        if (!_navAgent)
        {
            Debug.LogError("Failed to get NavMeshAgent on " + gameObject.name.ToString() + ", creating one now...");
            _navAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        _sphereCollider.radius = _sphereRadius;
        _sphereCollider.isTrigger = true;
        _navMeshPath = new NavMeshPath();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        //For drawing the NavMeshPath
        _pathElapsed += Time.deltaTime;

        if (_pathElapsed > 1.0f)
        {
            _pathElapsed -= 1.0f;
            NavMesh.CalculatePath(gameObject.transform.position, _lastKnownPos, NavMesh.AllAreas, _navMeshPath);
        }

        for (var i = 0; i < _navMeshPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(_navMeshPath.corners[i], _navMeshPath.corners[i + 1], Color.red);
        }

        SearchForTarget();
        
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Mob" && other.gameObject.name == "TankActor")
        {
            //The mob is within our LoS
            if (InLineOfSight(other))
            {
                //They're within our LoS, let's move to them
                MoveToTarget(other);
            }
            else
            {
                //We don't have clear LoS to the target
                //Let's atleast keep a reference to it should they suddenly come within our LoS
                SetPossibleTarget(other);
                return;
            }
        }

        //We heard a noise
        if (other.gameObject.tag == "Audible")
        {
            //Move to the source of said noise
            MoveToAudible(other);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mob" && other.gameObject.name == "TankActor")
        {
            //They're out of our search radius, ignore them
            Debug.Log(other.gameObject.ToString() + " has exited my search radius!");
            currentTarget = null;
            currentTargetCollider = null;
            audioTargetCollider = null;
        }
    }

    /// <summary>
    /// This is to check if we have an unobstructed LoS to whoever enters the monster's search radius
    /// If it's unobstructed, we know that we can "see" them
    /// </summary>
    private bool InLineOfSight(Collider col)
    {
        var startPos = gameObject.transform.position;
        var endPos = (col.transform.position - gameObject.transform.position);
        RaycastHit hit;
       
        if (Physics.Raycast(startPos, endPos, out hit) == true)
        {
            if (hit.collider.gameObject.name == col.gameObject.name)
            {
                Debug.DrawRay(startPos, hit.point - startPos, Color.green);
                return true;        
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// This is to search for targets within our radius
    /// </summary>
    private void SearchForTarget()
    {
        //We have a reference to someone inside of our radius
        if (currentTargetCollider)
        {
            //Checking to see if they are actually within our radius
            if (_sphereCollider.bounds.Contains(currentTargetCollider.bounds.center))
            {
                //Do we have a clear LoS to the target?
                //If so, move to it.
                if (InLineOfSight(currentTargetCollider))
                {
                    _navAgent.SetDestination(currentTargetCollider.transform.position);
                    _lastKnownPos = currentTarget.transform.position;
                }
            }
        }
        
    }

    /// <summary>
    /// Moves to our target
    /// </summary>
    /// <param name="other"></param>
    private void MoveToTarget(Collider other)
    {
        _navAgent.SetDestination(other.transform.position);
        currentTarget = other.gameObject;
        currentTargetCollider = other.gameObject.GetComponent<Collider>();
    }

    /// <summary>
    /// We have a target within our search radius
    /// We just don't have a clear LoS to them at this moment
    /// Let's keep checking to see if we get a clear LoS to them while they are within out radius
    /// </summary>
    /// <param name="other"></param>
    private void SetPossibleTarget(Collider other)
    {
        currentTarget = other.gameObject;
        currentTargetCollider = other.gameObject.GetComponent<Collider>();
        currentTarget = other.gameObject;
    }

    /// <summary>
    /// I heard a noise, let's move to the source of that noise.
    /// </summary>
    /// <param name="other"></param>
    private void MoveToAudible(Collider other)
    {
        _navAgent.SetDestination(other.transform.position);
        audioTargetCollider = other.gameObject.GetComponent<Collider>();
        currentTarget = other.gameObject;
        _lastKnownPos = other.gameObject.transform.position;
    }

    private void ListenForAudible()
    {
        if (audioTargetCollider)
        {
            if (_sphereCollider.bounds.Contains(audioTargetCollider.bounds.center))
            {
                _navAgent.SetDestination(audioTargetCollider.transform.position);
               _lastKnownPos = audioTargetCollider.transform.position;    
            }
        }
    }
}

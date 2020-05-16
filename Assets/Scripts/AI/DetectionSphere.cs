using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectionSphere : MonoBehaviour
{
 
    [Header("References")]
    public GameObject _currentTarget;
    public Collider _currentTargetCollider;
    public Collider _audioTargetCollider;

    private Vector3 _lastKnownPos;
    private SphereCollider _sphereCollider;
    private NavMeshAgent _navAgent;
    private NavMeshPath _navMeshPath;

    private float pathElapsed = 0.0f; //For NavMeshPath debugging purposes
    private float sphereRadius = 5.0f;
    private bool bHeardAudio = false; //Have we detected an audible source?
    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();

        if (!_sphereCollider)
        {
            Debug.LogError("Failed to get SphereCollider on " + gameObject.name.ToString() + ", creating one now...");
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
        }

        _sphereCollider.radius = sphereRadius;
        _sphereCollider.isTrigger = true;

        _navAgent = GetComponentInParent<NavMeshAgent>();

        if (!_navAgent)
        {
            Debug.LogError("Failed to get NavMeshAgent on " + gameObject.name.ToString() + ", creating one now...");
            _navAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        _navMeshPath = new NavMeshPath();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        pathElapsed += Time.deltaTime;

        if (pathElapsed > 1.0f)
        {
            pathElapsed -= 1.0f;
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
        if (other.gameObject.tag == "Mob")
        {
            if (InLineOfSight(other))
            {
                MoveToTarget(other);
            }
            else
            {
                SetPossibleTarget(other);
                return;
            }
        }

        if (other.gameObject.tag == "Audible")
        {
            bHeardAudio = true;
            MoveToAudible(other);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mob")
        {
            Debug.Log(other.gameObject.ToString() + " has exited my search radius!");
            _currentTarget = null;
            _currentTargetCollider = null;
            _audioTargetCollider = null;
        }
    }

    /// <summary>
    /// This is to check if we have an unobstructed LoS to whoever enters the monster's search radius
    /// If it's unobstructed, we know that we can "see" them
    /// </summary>
    /// <returns></returns>
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
                Debug.Log("I have an unobstructed Line of Sight to " + col.gameObject.name.ToString());
                return true;        
            }
            else
            {
                Debug.LogWarning("Line of Sight to " + col.gameObject.name.ToString() + " is obstructed!");
                return false;
            }
        }
        return false;
    }

    private void SearchForTarget()
    {
        if (_currentTargetCollider)
        {
            if (_sphereCollider.bounds.Contains(_currentTargetCollider.bounds.center))
            {
                if (InLineOfSight(_currentTargetCollider))
                {
                    _navAgent.SetDestination(_currentTargetCollider.transform.position);
                    _lastKnownPos = _currentTarget.transform.position;
                }
            }
        }
        
    }

    private void MoveToTarget(Collider other)
    {
        _navAgent.SetDestination(other.transform.position);
        _currentTarget = other.gameObject;
        _currentTargetCollider = other.gameObject.GetComponent<Collider>();
    }

    private void SetPossibleTarget(Collider other)
    {
        _currentTarget = other.gameObject;
        _currentTargetCollider = other.gameObject.GetComponent<Collider>();
        _currentTarget = other.gameObject;
    }

    private void MoveToAudible(Collider other)
    {
        _navAgent.SetDestination(other.transform.position);
        _audioTargetCollider = other.gameObject.GetComponent<Collider>();
        _currentTarget = other.gameObject;
        _lastKnownPos = other.gameObject.transform.position;
    }

    private void ListenForAudible()
    {
        if (_audioTargetCollider)
        {
            if (_sphereCollider.bounds.Contains(_audioTargetCollider.bounds.center))
            {
                _navAgent.SetDestination(_audioTargetCollider.transform.position);
               _lastKnownPos = _audioTargetCollider.transform.position;    
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectionSphere : MonoBehaviour
{
    enum MONSTER_STATE
    {
        NONE,
        IDLE,
        MOVING,
        MOVING_TO_TARGET,
        ATTACKING_TARGET
    }

    [Header("References")]
    public Transform NavGoal;
    public GameObject _currentTarget;

    private Vector3 _lastKnownPos;
    private SphereCollider _sphereCollider;
    private NavMeshAgent _navAgent;
    private NavMeshPath _navMeshPath;
    private MONSTER_STATE monsterState;
    private float pathElapsed = 0.0f; //For NavMeshPath debugging purposes


    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();

        if (!_sphereCollider)
        {
            Debug.LogError("Failed to get SphereCollider on " + gameObject.name.ToString() + ", creating one now...");
            _sphereCollider = gameObject.AddComponent<SphereCollider>();
        }

        _navAgent = GetComponent<NavMeshAgent>();
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
        _navAgent.destination = NavGoal.position;
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

        var targetCol = _currentTarget.gameObject.GetComponent<Collider>();

        if(_sphereCollider.bounds.Contains(targetCol.bounds.center))
        {
            if (InLineOfSight(targetCol))
            {
                _navAgent.SetDestination(targetCol.transform.position);
                _lastKnownPos = _currentTarget.transform.position;   
            }
        }

       Debug.Log(_lastKnownPos.ToString());
       
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

                _navAgent.SetDestination(other.transform.position);
                _currentTarget = other.gameObject;
            }
            else
            {
                return;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Mob")
        {
            Debug.Log(other.gameObject.ToString() + " has exited my search radius!");
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

}

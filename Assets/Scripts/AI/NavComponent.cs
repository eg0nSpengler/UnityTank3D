using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class holds all navigation related logic for AI Mobs
/// </summary>
/// 

[RequireComponent(typeof(NavMeshAgent))]
//[RequireComponent(typeof(DetectionSphere))]
[RequireComponent(typeof(HealthComponent))]
public class NavComponent : MonoBehaviour
{
    public delegate void DestinationBegin();
    public delegate void DestinationSuccess();
    public delegate void DestinationFail();

    public delegate void NavToTarget(GameObject obj);

    /// <summary>
    /// Called when the AI begins navigating to it's destination
    /// </summary>
    public event DestinationBegin OnDestinationBegin;

    /// <summary>
    /// Called when the AI successfully reaches it's destination
    /// </summary>
    public event DestinationSuccess OnDestinationSuccess;

    /// <summary>
    /// Called when the AI fails to reach it's destination
    /// </summary>
    public event DestinationFail OnDestinationFail;

    /// <summary>
    /// Called when the AI navigates to a target
    /// </summary>
    public event NavToTarget OnNavToTarget;

    private NavMeshAgent _navAgent;
    //private DetectionSphere //_detectionsphere;
    private NavMeshPath _navMeshPath;
    private GameObject _currentTarget;
    private Vector3 _lastKnownPos;
    private HealthComponent _healthComp;
    private IEnumerator _coroutineRef; // This is to keep a ref to our running coroutine

    private float _pathElapsed; //For NavMeshPath debugging purposes
    private float _moveSpeed;

    private void Awake()
    {
        _navAgent = GetComponentInParent<NavMeshAgent>();
        //_detectionsphere = GetComponentInChildren<DetectionSphere>();

        _navMeshPath = new NavMeshPath();
        _currentTarget = null;
        _lastKnownPos = new Vector3(0.0f, 0.0f, 0.0f);
        _moveSpeed = 2f;
        _coroutineRef = CheckDist();
        _pathElapsed = 0.0f;
        _navAgent.speed = _moveSpeed;

        if (GetComponentInParent<HealthComponent>() != null)
        {
            Debug.Log("NavComponent has found a health component!");
            _healthComp = GetComponentInParent<HealthComponent>();
            _healthComp.OnHealthZero += StopNav;
        }
        
        if (!_navAgent)
        {
            Debug.LogError("Failed to get NavMeshAgent on " + gameObject.name.ToString() + ", creating one now");
            _navAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        /*if (!//_detectionsphere)
        {
            Debug.LogError("Failed to get DetectionSphere on " + gameObject.name.ToString() + ", creating one now");
            //_detectionsphere = gameObject.AddComponent<DetectionSphere>();
        }*/


        
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        //_detectionsphere.OnTargetClearLOS += MoveToTarget;
        //_detectionsphere.OnTargetInRadius += SetPossibleTarget;
        //_detectionsphere.OnTargetExitRadius += ForgetTarget;
        //_detectionsphere.OnTargetExitRadius += MoveToLastKnownPos;
        //_detectionsphere.OnHeardSound += MoveToAudible;
        //_detectionsphere.OnTargetTracking += MoveToTarget;
    }

    private void OnDisable()
    {

        //_detectionsphere.OnTargetClearLOS -= MoveToTarget;
        //_detectionsphere.OnTargetInRadius -= SetPossibleTarget;
        //_detectionsphere.OnTargetExitRadius -= ForgetTarget;
        //_detectionsphere.OnTargetExitRadius -= MoveToLastKnownPos;
        //_detectionsphere.OnHeardSound -= MoveToAudible;
        //_detectionsphere.OnTargetTracking -= MoveToTarget;

        OnDestinationBegin -= UpdateDestStatus;

        StopAllCoroutines();
    }

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

    }

    /// <summary>
    /// Moves to our target
    /// </summary>
    private void MoveToTarget(GameObject obj)
    {
        _currentTarget = obj;
        _navAgent.SetDestination(obj.transform.position);
        OnDestinationBegin?.Invoke();
    }

    /// <summary>
    /// We have a target within our search radius
    /// We just don't have a clear LoS to them at this moment
    /// Let's keep checking to see if we get a clear LoS to them while they are within out radius
    /// </summary>
    private void SetPossibleTarget(GameObject obj)
    {
        _currentTarget = obj;
    }

    private void ForgetTarget(GameObject obj)
    {
        _lastKnownPos = _currentTarget.transform.position;
        _currentTarget = null;
    }

    private void MoveToLastKnownPos(GameObject obj)
    {
        _navAgent.SetDestination(_lastKnownPos);
        OnDestinationBegin?.Invoke();
    }

    /// <summary>
    /// I heard a noise, let's move to the source of that noise.
    /// </summary>
    /// <param name="other"></param>
    private void MoveToAudible(GameObject obj)
    {
        if (_navAgent.enabled == true)
        {
            _navAgent.SetDestination(obj.transform.position);
            _currentTarget = obj;
            _lastKnownPos = obj.transform.position;
            OnDestinationBegin?.Invoke();    
        }
    }

    /// <summary>
    /// So the monster's corpse doesn't navigate.
    /// <para>lol</para>
    /// </summary>
    void StopNav()
    {
        
        StopCoroutine(_coroutineRef);
        _navAgent.isStopped = true;
        _navAgent.enabled = false;
        OnDestinationFail?.Invoke();
    }

    IEnumerator CheckDist()
    {
        // I don't know why but
        // As great as predicates are
        // That little () => bit of syntax has always irked me
        // Like I see stray character(s) in code and my eyes dart to it's immediately
        // Oh wait a minute it's just a predicate, nah it's cool
        // It'd be like if you went walking around sipping from brown glass bottle
        // People would obv think you'd be chugging down a cold one
        // But wait a minute what's this?
        // They see the label and see that its a cold one alright...
        // A cold bottle of non-alcoholic Apple Cider
        // Yep

        yield return new WaitWhile(() => _navAgent.remainingDistance > _navAgent.stoppingDistance);

        OnDestinationSuccess?.Invoke();
    }

    /// <summary>
    /// This is to keep checking the distance between us and the destination while we're navigating
    /// </summary>
    private void UpdateDestStatus()
    {
        StartCoroutine(_coroutineRef);   
    }

}

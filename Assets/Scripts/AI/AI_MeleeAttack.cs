using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_MeleeAttack : MonoBehaviour
{
    [Header("Variables")]
    public int damageAmount;

    public delegate void DamageMob(GameObject obj, int dmg);

    /// <summary>
    /// Called when a mob is hit by our melee attack
    /// </summary>
    public static event DamageMob OnDamageMobEvent;

    private NavComponent _navComponent;
    private GameObject _targetToAttack;
    private bool HasAttacked;

    private void Awake()
    {
        _navComponent = GetComponent<NavComponent>();
        damageAmount = 2;
        HasAttacked = false;

        if(!_navComponent)
        {
            Debug.Log("Failed to get Nav Component " + gameObject.name.ToString() + " creating one now.");
            _navComponent = gameObject.AddComponent<NavComponent>();
        }

        _navComponent.OnDestinationSuccess += SearchForTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnDisable()
    {
        _navComponent.OnDestinationSuccess -= SearchForTarget;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SearchForTarget()
    {
        var startPos = gameObject.transform.position;
        var endPos = gameObject.transform.forward * 10.0f;
        RaycastHit hit;

        if (Physics.Raycast(startPos, endPos, out hit) == true)
        {
            _targetToAttack = hit.collider.gameObject;
        }

        if (_targetToAttack)
        {
            if (HasAttacked == false)
            {
                OnDamageMobEvent(_targetToAttack, damageAmount);
                HasAttacked = true;
            }
            else
            {
                StartCoroutine(AttackTimer());
            }
        }
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(3);
        HasAttacked = false;
    }

}

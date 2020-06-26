﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton to manage all Mobs within a level
/// </summary>
public class MobManager : MonoBehaviour
{

    public delegate void MobDestroyed();

    /// <summary>
    /// Called when a mob is "destroyed" and disabled in scene
    /// </summary>
    public static event MobDestroyed OnMobDestroyed;

    private static List<GameObject> _mobList;
    private static Vector3 _lastDestroyedPos;

    private void Awake()
    {
        _mobList = new List<GameObject>();
        _lastDestroyedPos = new Vector3(0.0f, 0.0f, 0.0f);

        foreach (var mob in FindObjectsOfType<GameObject>())
        {
            if (mob.tag == TagStatics.GetMobTag() && mob.name != TagStatics.GetPlayerName())
            {
                _mobList.Add(mob);
                if (mob.GetComponent<HealthComponent>() != null)
                {
                    mob.GetComponent<HealthComponent>().OnHealthZero += UpdateMobList;
                }
                else
                {
                    Debug.LogError("Failed to get a Health Component on " + mob.gameObject.name.ToString());
                }
            }
        }

        Debug.Log("The MobList in MobManager contains " + _mobList.Count.ToString() + " mob(s)");

        if (_mobList.Count <= 0)
        {
            Debug.LogWarning("MobManger didn't find any valid Mobs within the current scene");
        }

        ProjectileHandler.OnDamageMobEvent += DamageMob;
        AI_MeleeAttack.OnDamageMobEvent += DamageMob;
    }

    private void OnDisable()
    {
        foreach (var mob in _mobList)
        {
            mob.GetComponent<HealthComponent>().OnHealthZero -= UpdateMobList;
        }

        ProjectileHandler.OnDamageMobEvent -= DamageMob;
        AI_MeleeAttack.OnDamageMobEvent -= DamageMob;
    }
    
   private static void DamageMob(GameObject obj, int dmg)
    {
        if(obj.tag != TagStatics.GetMobTag())
        {
            Debug.LogError("Failed to call DamageMob on " + obj.name.ToString() + " the GameObject may be an untagged Mob or not a Mob at all.");
            return;
        }

        var objHealthComp = obj.gameObject.GetComponent<HealthComponent>();

        if (!objHealthComp)
        {
            Debug.LogError("Failed to get a Health Component on " + obj.name.ToString() + " the GameObject may be a Mob without a Health Component or not a Mob at all.");
            return;
        }
        else
        {
            objHealthComp.TakeDamage(dmg);
            Debug.Log(obj.name.ToString() + " has been dealt " + dmg.ToString() + " damage");
        }
    }

    private static void HealMob(GameObject obj, int hp)
    {
        if (obj.tag != TagStatics.GetMobTag())
        {
            Debug.LogError("Failed to call HealMob on " + obj.name.ToString() + " the GameObject may be an untagged Mob or not a Mob at all.");
            return;
        }

        var objHealthComp = obj.gameObject.GetComponent<HealthComponent>();

        if (!objHealthComp)
        {
            Debug.LogError("Failed to get a Health Component on " + obj.name.ToString() + " the GameObject may be a Mob without a Health Component or not a Mob at all.");
            return;
        }
        else
        {
            objHealthComp.Heal(hp);
            Debug.Log(obj.name.ToString() + " has been healed for " + hp.ToString() + " hitpoints");
        }
    }

    private void UpdateMobList()
    {
        Debug.Log("UpdateMobList called!");

        // Again
        // We do a container copy because modifying a container while iterating over it is again...
        // A GIGANTIC NO-NO
        // BAD
        // DON'T DO IT
        // UNLESS YA WANNA FIGHT
        foreach (var mob in _mobList.ToArray())
        {
            // Like in the PickupManager class
            // We know if a mob is "dead" if it's disabled in scene
            if (mob.gameObject.activeInHierarchy == false)
            {
                _lastDestroyedPos = mob.transform.position;
                _mobList.Remove(mob);
                Debug.Log("The Mob list now contains " + _mobList.Count.ToString() + " mob(s)");
            }
        }

        OnMobDestroyed();
    }
    /// <summary>
    /// Returns each Mob Position in the level
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Vector3> GetMobPositions()
    {
        foreach (var mob in _mobList)
        {
            yield return mob.transform.position;
        }
    }

    /// <summary>
    /// Returns the position of the most recently destroyed mob
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetRecentMobDestroyedPos()
    {
        Debug.Log(_lastDestroyedPos.ToString());
        return _lastDestroyedPos;
    }

}

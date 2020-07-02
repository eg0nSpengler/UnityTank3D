﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton to manage all Mobs within a level
/// </summary>
public class MobManager : MonoBehaviour
{

    private void Awake()
    {
        ProjectileHandler.OnDamageMobEvent += DamageMob;
    }

    private void OnDisable()
    {
        ProjectileHandler.OnDamageMobEvent -= DamageMob;
    }
    
   private static void DamageMob(GameObject obj, int dmg)
    {
        if(obj.tag != "Mob")
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
        if (obj.tag != "Mob")
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


}

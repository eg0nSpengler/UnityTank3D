using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singleton to manage all Mobs within a level
/// </summary>
public class MobManager : MonoBehaviour
{

    private static OnMobDamage OnMobDamageDelegate;
    private static OnMobHeal OnMobHealDelegate;

    private delegate void OnMobDamage(GameObject obj, int dmg);
    private delegate void OnMobHeal(GameObject obj, int hp);

    private void Awake()
    {

        OnMobDamageDelegate = DamageMob;
        OnMobHealDelegate = HealMob;

        ProjectileHandler.OnDamageMobEvent += DamageMob;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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

    private static void GetMobGunStatus(GameObject obj)
    {
        if (obj.tag != "Mob")
        {
            Debug.LogError("Failed to call GetMobGunStatus on " + obj.name.ToString() + " the Gameobject may be an untagged Mob or not a Mob at all");
            return;
        }

        var objGunComp = obj.gameObject.GetComponent<TankGun>();

    }

}

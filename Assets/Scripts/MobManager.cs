using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    private static List<GameObject> _mobList;

    private static OnMobDamage OnMobDamageDelegate;
    private static OnMobHeal OnMobHealDelegate;

    private delegate void OnMobDamage(GameObject obj, int dmg);
    private delegate void OnMobHeal(GameObject obj, int hp);

    private void Awake()
    {
        _mobList = new List<GameObject>();

       foreach(var obj in FindObjectsOfType<GameObject>())
       {
            if (obj.tag == "Mob")
            {
                _mobList.Add(obj);
            }
       }

        Debug.Log("The Mob List in the Mob Manger class currently contains " + _mobList.Count.ToString() + " mob(s)");

        OnMobDamageDelegate = DamageMob;
        OnMobHealDelegate = HealMob;

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
        var objHealthComp = obj.gameObject.GetComponent<HealthComponent>();

        if (!objHealthComp)
        {
            Debug.LogError("Failed to get a Health Component in Mob Manager!");
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
        var objHealthComp = obj.gameObject.GetComponent<HealthComponent>();

        if (!objHealthComp)
        {
            Debug.LogError("Failed to get a Health Component in Mob Manager!");
            return;
        }
        else
        {
            objHealthComp.Heal(hp);
            Debug.Log(obj.name.ToString() + " has been healed for " + hp.ToString() + " hitpoints");
        }
    }

    public static void InvokeMobTakeDamage(GameObject obj, int dmg)
    {
        OnMobDamageDelegate.Invoke(obj, dmg);
    }

    public static void InvokeMobHeal(GameObject obj, int hp)
    {
        OnMobHealDelegate.Invoke(obj, hp);
    }

}

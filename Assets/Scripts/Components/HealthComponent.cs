using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Variables")]
    public int currentHP;


    public delegate void HealthModified();
    public delegate void HealthZero();

    /// <summary>
    /// Called whenever the health is modified (Damage taken/healing/etc)
    /// </summary>
    public event HealthModified OnHealthModified;

    /// <summary>
    /// Called when the current HP reaches zero
    /// </summary>
    public event HealthZero OnHealthZero;

    private int _maxHP;

    private void Awake()
    {
        _maxHP = currentHP;   

        if (gameObject.tag != TagStatics.GetMobTag())
        {
            Debug.LogWarning(gameObject.name.ToString() + " has a HealthComponent but is not tagged as a Mob!");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
    }



    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Damages the Mob for X amount of health
    /// </summary>
    /// <param name="dmg"></param>
    public void TakeDamage(int dmg)
    {
        if (currentHP <= 0)
        {
            Debug.LogWarning("Cannot call TakeDamage on " + gameObject.name.ToString() + " current HP is " + currentHP.ToString());
            return;
        }
        else
        {
            if (currentHP - dmg < 0)
            {
                currentHP = 0;
            }
            else
            {
                currentHP -= dmg;    
                OnHealthModified();
            }

        }

        if (currentHP <= 0)
        {
            OnHealthZero();
        }

    }

    /// <summary>
    /// Heals the Mob for X amount of health
    /// </summary>
    /// <param name="hp"></param>
    public void Heal(int hp)
    {
        if (currentHP >= _maxHP)
        {
            Debug.LogWarning("Cannot call Heal on " + gameObject.name.ToString() + " current HP is at maximum value of " + _maxHP.ToString());
        }
        else
        {
            if (currentHP + hp > _maxHP)
            {
                currentHP = _maxHP;
            }
            else
            {
                currentHP += hp;
                OnHealthModified();
            }
        }

    }

    /// <summary>
    /// Returns the current Health
    /// </summary>
    /// <returns></returns>
    public int GetHealth()
    {
        return currentHP;
    }

    /// <summary>
    /// Returns the max Health
    /// </summary>
    /// <returns></returns>
    public int GetMaxHealth()
    {
        return _maxHP;
    }

}

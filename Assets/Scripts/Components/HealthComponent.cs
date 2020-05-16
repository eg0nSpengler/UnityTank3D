using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Variables")]
    public int currentHP;

    private int _maxHP;
    private void Awake()
    {
        if (gameObject.tag != "Mob")
        {
            Debug.LogWarning(gameObject.name.ToString() + " has a HealthComponent but is not tagged as a Mob!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _maxHP = currentHP;   
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
            }

        }

        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
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
            }
        }
    }
}

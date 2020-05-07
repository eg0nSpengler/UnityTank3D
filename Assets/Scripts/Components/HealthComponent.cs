using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [Header("Variables")]
    public int currentHP;

    private int maxHP;
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHP = currentHP;   
    }

    // Update is called once per frame
    void Update()
    {

    }


    /// <summary>
    /// Damages the GameObject for X amount of health
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
                Debug.LogWarning(gameObject.name.ToString() + "'s health component has taken " + dmg.ToString() + " damage!");
                currentHP -= dmg;    
            }

        }

        if (currentHP <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Heals the GameObject for X amount of health
    /// </summary>
    /// <param name="hp"></param>
    public void Heal(int hp)
    {
        if (currentHP >= maxHP)
        {
            Debug.LogWarning("Cannot call Heal on " + gameObject.name.ToString() + " current HP is at maximum value of " + maxHP.ToString());
        }
        else
        {
            if (currentHP + hp > maxHP)
            {
                currentHP = maxHP;
            }
            else
            {
                currentHP += hp;
            }
        }
    }
}

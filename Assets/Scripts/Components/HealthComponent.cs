using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
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

    // Did I ever tell you how much I hate writing backing fields?

    /// <summary>
    /// The current HP of the mob
    /// </summary>
    public int CurrentHP
    {
        get { return _currentHP;}
        set { _currentHP = value; }
    }

    /// <summary>
    /// Is the mob dead?
    /// </summary>
    public bool IsDead { protected set; get; }


    /// <summary>
    /// The maximum HP of the mob
    /// </summary>
    public int MaxHP { protected set; get; }

    /// <summary>
    /// The current HP of the mob
    /// </summary>
    [SerializeField]
    private int _currentHP;

    private void Awake()
    {
        MaxHP = CurrentHP;
        IsDead = false;

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
        if (CurrentHP <= 0)
        {
            Debug.LogWarning("Cannot call TakeDamage on " + gameObject.name.ToString() + " current HP is " + CurrentHP.ToString());
            return;
        }
        else
        {
            if (CurrentHP - dmg < 0)
            {
                CurrentHP = 0;
            }
            else
            {
                CurrentHP -= dmg;    
                //OnHealthModified();
            }

        }

        if (CurrentHP <= 0)
        {
            IsDead = true;
            OnHealthZero();
        }

    }

    /// <summary>
    /// Heals the Mob for X amount of health
    /// </summary>
    /// <param name="hp"></param>
    public void Heal(int hp)
    {
        if (CurrentHP >= MaxHP)
        {
            Debug.LogWarning("Cannot call Heal on " + gameObject.name.ToString() + " current HP is at maximum value of " + MaxHP.ToString());
        }
        else
        {
            if (CurrentHP + hp > MaxHP)
            {
                CurrentHP = MaxHP;
            }
            else
            {
                CurrentHP += hp;
                OnHealthModified();
            }
        }

    }

}

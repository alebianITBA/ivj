using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, Life<Minion>
{
    private int health;

    void Start ()
    {
        this.health = Constants.MINION_MAX_BASE_HEALTH;
    }

    void Update ()
    {
		
    }

    public int Heal (int amount)
    {
        return 0;
    }

    public int TakeDamage (int amount)
    {
        if (health > 0) {
            this.health--;
            return amount;
        }
        else {
            return 0;
        }
    }

    public int GetTotalHealth ()
    {
        return Constants.MINION_MAX_BASE_HEALTH;
    }

    public int GetCurrentHealth ()
    {
        return this.health;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private int health;
    public Sprite[] sprites;

    void Start ()
    {
        this.health = Constants.TOWER_MAX_BASE_HEALTH;
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
        return Constants.TOWER_MAX_BASE_HEALTH;
    }

    public int GetCurrentHealth ()
    {
        return this.health;
    }
}

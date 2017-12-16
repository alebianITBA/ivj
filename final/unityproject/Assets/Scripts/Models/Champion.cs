using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour, Life<Champion>
{

    private int health;

    void Start ()
    {
        this.health = Constants.PLAYER_MAX_BASE_HEALTH;
    }

    void Update ()
    {
		
    }

    public int Heal (int amount)
    {
        int healedAmount = 0;
        int newHealth = health + amount;
        if (newHealth > GetTotalHealth()) {
            health = GetTotalHealth();
            int reminder = newHealth - GetTotalHealth();
            healedAmount = Mathf.Abs(reminder - amount);
        }
        else {
            health = newHealth;
            healedAmount = amount;
        }
        return healedAmount;
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
        return Constants.PLAYER_MAX_BASE_HEALTH;
    }

    public int GetCurrentHealth ()
    {
        return this.health;
    }
}

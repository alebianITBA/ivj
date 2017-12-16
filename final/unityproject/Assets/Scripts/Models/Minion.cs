using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour, Life<Minion>, Team
{
    private float health;
    public GameManager.Teams team;

    void Start ()
    {
        this.health = Constants.MINION_MAX_BASE_HEALTH;
    }

    void Update ()
    {
		
    }

    public float Heal (float amount)
    {
        return 0.0f;
    }

    public float TakeDamage (float amount)
    {
        if (health > 0) {
            this.health -= amount;
            return amount;
        }
        else {
            return 0.0f;
        }
    }

    public float GetTotalHealth ()
    {
        return Constants.MINION_MAX_BASE_HEALTH;
    }

    public float GetCurrentHealth ()
    {
        return this.health;
    }

    public bool IsRED ()
    {
        return this.team == GameManager.Teams.RED;
    }

    public bool IsBLUE ()
    {
        return this.team == GameManager.Teams.BLUE;
    }

    public GameManager.Teams GetTeam ()
    {
        return this.team;
    }

    public void SetTeam (GameManager.Teams team)
    {
        this.team = team;
    }
}

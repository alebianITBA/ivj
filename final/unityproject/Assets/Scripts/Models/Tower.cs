using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour, Life<Tower>, Team
{
    private float health;
    public Sprite[] sprites;
    public GameObject healthBar;
    public GameManager.Teams team;

    void Start ()
    {
        this.health = Constants.TOWER_MAX_BASE_HEALTH;
    }

    void Update ()
    {
        if (healthBar != null) {
            healthBar.transform.localScale = new Vector3(GetCurrentHealth() / GetTotalHealth(), 1.0f, 1.0f);
        }
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
        return Constants.TOWER_MAX_BASE_HEALTH;
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

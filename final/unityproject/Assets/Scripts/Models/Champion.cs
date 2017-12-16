using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour, Life<Champion>, Team
{
    protected int health;
    protected Rigidbody2D rb;
    public GameManager.Teams team;
    protected GameObject camera;

    protected Vector2 direction ()
    {
        float angle = transform.localRotation.eulerAngles.z;
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }

    public void SetCamera (GameObject camera)
    {
        this.camera = camera;
    }

    protected void applyImpulseForward ()
    {
        Vector2 forward = direction();
        rb.AddForce(forward * GetVelocity());
    }

    protected void applyImpulseBackwards ()
    {
        Vector2 forward = direction();
        rb.AddForce(-1 * forward * GetVelocity());
    }

    protected float GetVelocity ()
    {
        return Constants.PLAYER_BASE_VELOCITY;
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

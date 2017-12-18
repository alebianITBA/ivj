using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour, Life<Champion>, Team
{
    protected float health;
    protected Rigidbody2D rb;
    public GameManager.Teams team;
    protected GameObject cam;

    protected Vector2 direction ()
    {
        float angle = transform.localRotation.eulerAngles.z;
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }

    protected Vector2 direction (float deviation)
    {
        float angle = transform.localRotation.eulerAngles.z + deviation;
        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector2(x, y);
    }

    public void SetCamera (GameObject cam)
    {
        this.cam = cam;
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

    public float Heal (float amount)
    {
        float healedAmount = 0;
        float newHealth = health + amount;
        if (newHealth > GetTotalHealth()) {
            health = GetTotalHealth();
            float reminder = newHealth - GetTotalHealth();
            healedAmount = Mathf.Abs(reminder - amount);
        }
        else {
            health = newHealth;
            healedAmount = amount;
        }
        return healedAmount;
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
        return Constants.PLAYER_MAX_BASE_HEALTH;
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

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.name == "MinionSpawn") {
            col.gameObject.GetComponent<MinionSpawn>().Activate(GetComponent<Champion>());
            col.gameObject.GetComponent<MinionSpawn>().ReleaseMinions();
        }
        if (col.gameObject.name == "Bullet") {
            Bullet bul = col.gameObject.GetComponent<Bullet>();
            if (bul.GetTeam() != GetTeam()) {
                bul.Recycle();
                TakeDamage(bul.GetDamage());
            }
        }
        if (col.gameObject.name == "Rocket") {
            Rocket rocket = col.gameObject.GetComponent<Rocket>();
            if (rocket.GetTeam() != GetTeam()) {
                TakeDamage(rocket.GetDamage());
            }
            rocket.Recycle();
        }
    }
}

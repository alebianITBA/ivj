using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, Life<Base>, Team
{
    public GameManager.Teams team;
    private float health;
    public GameObject healthBar;

    void Start ()
    {
        this.health = Constants.BASE_MAX_BASE_HEALTH;
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
            return 0;
        }
    }

    public float GetTotalHealth ()
    {
        return Constants.BASE_MAX_BASE_HEALTH;
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
        if (col.gameObject.name == "Bullet") {
            Bullet bul = col.gameObject.GetComponent<Bullet>();
            bul.Recycle();
            if (bul.GetTeam() != GetTeam()) {
                TakeDamage(bul.GetDamage() * 3);
            }
        }
    }
}

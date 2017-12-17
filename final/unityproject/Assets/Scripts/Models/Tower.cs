using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour, Life<Tower>, Team
{
    private float health;
    public Sprite[] sprites;
    public GameObject healthBar;
    public GameManager.Teams team;
    public bool alive;

    void Start ()
    {
        this.health = Constants.TOWER_MAX_BASE_HEALTH;
        this.alive = true;
    }

    void Update ()
    {
        if (healthBar != null) {
            if (health > 0) {
                healthBar.transform.localScale = new Vector3(GetCurrentHealth() / GetTotalHealth(), 1.0f, 1.0f);
            }
            else {
                healthBar.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                healthBar.GetComponent<Image>().color = Color.gray;
            }
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
            this.alive = false;
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

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.name == "Bullet") {
            Bullet bul = col.gameObject.GetComponent<Bullet>();
            bul.Recycle();
            if (bul.GetTeam() != GetTeam()) {
                TakeDamage(bul.GetDamage() * 5);
            }
        }
    }
}

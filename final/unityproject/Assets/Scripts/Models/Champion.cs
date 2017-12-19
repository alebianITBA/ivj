using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : MonoBehaviour, Life<Champion>, Team
{
    protected float health;
    protected bool alive = true;
    protected System.DateTime deadAt;
    protected Rigidbody2D rb;
    public GameManager.Teams team;
    protected GameObject cam;

    void FixedUpdate ()
    {
        RespawnIfDead();
    }

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
            float damageDealed = 0.0f;
            float newHealth = health - amount;
            if (newHealth > 0.0f) {
                this.health = newHealth;
                damageDealed = amount;
            }
            else {
                this.alive = false;
                Disable();
                this.deadAt = System.DateTime.Now;
                TakeToBase();
                SoundManager.PlaySound((int)SndIdGame.DEAD);
                damageDealed = health;
                health = 0.0f;
            }
            return damageDealed;
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
    }

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Rocket") {
			Rocket rocket = col.gameObject.GetComponent<Rocket>();
			if (rocket.GetTeam() != GetTeam()) {
				TakeDamage(rocket.GetDamage());
			}
			rocket.Recycle();
		}
	}

    private void TakeToBase ()
    {
        gameObject.transform.position = IsRED() ? GameManager.Instance.RedTeamSpawn.GetCoordinates() : GameManager.Instance.BlueTeamSpawn.GetCoordinates();
    }

    protected void RespawnIfDead ()
    {
        if (alive) {
            Base myBase = IsRED() ? GameManager.Instance.REDBase : GameManager.Instance.BLUEBase;
            if (Constants.CloseEnough(myBase.gameObject, gameObject)) {
                Heal(1.0f);
            }
        }
        else {
            if ((System.DateTime.Now - deadAt).TotalMilliseconds > 5000.0f) {
                TakeToBase();
                this.health = Constants.PLAYER_MAX_BASE_HEALTH;
                Enable();
                this.alive = true;
            }
        }
    }

    protected void Disable ()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    protected void Enable ()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
    ZombieManager zombieManager;
    Rigidbody2D rb;
    public Animator animator;
	public Transform player;
    public System.DateTime died;
	private bool dead;
	public Sprite idle;
	public GameLogic gameLogic;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator> ();
        dead = false;
		GetComponent<SpriteRenderer> ().sprite = idle;
    }

	public void SetManager(ZombieManager manager) {
		zombieManager = manager;
	}

	public void SetAlive() {
		GetComponent<SpriteRenderer> ().sprite = idle;
		this.dead = false;
	}

	// Update is called once per frame
	void Update () {
        if (dead) {
			if ((System.DateTime.Now - died).TotalMilliseconds > gameLogic.ZombieTimeSpawn()) {
                zombieManager.RecycleZombie(this);
            }
            return;
        }
		Vector2 playerPosition = player.position;
        Vector2 myPosition = transform.position;
        Vector2 direction = playerPosition - myPosition;
		rb.AddForce(direction.normalized * gameLogic.ZombieVelocity());
    }

    void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Bullet"  && !dead) {
			dead = true;
			animator.SetBool("dead", dead);
            died = System.DateTime.Now;
			GetComponent<PolygonCollider2D> ().enabled = false;
			gameLogic.ZombieKilled ();
        }
    }
}

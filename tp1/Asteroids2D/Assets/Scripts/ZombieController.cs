using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
    private GameObject player;
    ZombieManager zombieManager;
    Rigidbody2D rb;
    public Animator animator;
    public System.DateTime died;
    public bool dead;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator> ();
        dead = false;
    }

	public void SetManager(ZombieManager manager) {
		zombieManager = manager;
	}

	// Update is called once per frame
	void Update () {
        if (dead) {
			if ((System.DateTime.Now - died).TotalMilliseconds > GameLogic.ZOMBIE_SHOW_BODY) {
                zombieManager.RecycleZombie(this);
            }
            return;
        }
        Vector2 playerPosition = GameObject.Find("Character").transform.position;
        Vector2 myPosition = transform.position;
        Vector2 direction = playerPosition - myPosition;
		rb.AddForce(direction.normalized * GameLogic.ZOMBIE_VELOCITY);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name == "Bullet") {
            animator.SetBool("dead", true);
            dead = true;
            died = System.DateTime.Now;
			GetComponent<PolygonCollider2D> ().enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
    private GameObject player;
    ZombieManager zombieManager;
    Rigidbody2D rb;
    Animator animator;
    public System.DateTime died;
    bool dead;
    float VELOCITY = 10f;
    float SHOW_BODY = 3000.0f;

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
            if ((System.DateTime.Now - died).TotalMilliseconds > SHOW_BODY) {
                zombieManager.RecycleZombie(this);
            }
            return;
        }
        Vector2 playerPosition = GameObject.Find("Character").transform.position;
        Vector2 myPosition = transform.position;
        Vector2 direction = playerPosition - myPosition;
        rb.AddForce(direction.normalized * VELOCITY);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name == "Bullet") {
            print("DEAD");
            animator.SetBool("dead", true);
            dead = true;
            died = System.DateTime.Now;
        }
    }
}

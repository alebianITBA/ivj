using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour {
	WarriorManager warriorManager;
    Rigidbody2D rb;
    public Animator animator;
    public System.DateTime died;
	private bool dead;
    [HideInInspector]
	private bool walking;
	private bool attacking;
	public Sprite idle;

    public GameObject container;
    public bool inQueue;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator> ();
        dead = false;
        inQueue = true;
		GetComponent<SpriteRenderer> ().sprite = idle;
		animator.SetBool ("attacking", false);
		animator.SetBool ("walking", false);
    }

	public void SetManager(WarriorManager manager) {
		warriorManager = manager;
	}

	public void SetAlive() {
		GetComponent<SpriteRenderer> ().sprite = idle;
		this.dead = false;
		animator.SetBool ("attacking", false);
		animator.SetBool ("walking", false);
	}

	// Update is called once per frame
	void Update () {
        if (dead) {
			if ((System.DateTime.Now - died).TotalMilliseconds > GameLogic.WARRIOR_SHOW_BODY) {
                warriorManager.RecycleWarrior(this);
            }
            return;
        }
		Vector2 playerPosition = CrazyCaveGameManager.Instance.player.transform.position;
        Vector2 myPosition = transform.position;
        Vector2 direction = playerPosition - myPosition;
		RaycastHit2D hit = Physics2D.Raycast ((Vector2)transform.position, direction, 100.0f);
		if (hit.transform.gameObject.name.Equals("Player(Clone)")) {
			if (hit.distance < 0.7f) {
				animator.SetBool ("attacking", true);
			} else {
				animator.SetBool ("attacking", false);
			}
			Vector2 velocity = this.rb.velocity;
			float angle = Mathf.Atan2 (velocity.y, velocity.x);
			transform.localRotation = Quaternion.Euler (0.0f, 0.0f, Mathf.Rad2Deg * angle - 90);
			animator.SetBool ("walking", true);
			rb.AddForce(direction.normalized * GameLogic.Instance.WarriorVelocity());
			return;
		}
		animator.SetBool ("walking", false);
    }

    void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Bullet"  && !dead) {
			dead = true;
			animator.SetBool ("dead", true);
			animator.SetBool("walking", false);
			animator.SetBool ("attacking", false);
            died = System.DateTime.Now;
            GetComponent<Collider2D> ().enabled = false;
			GameLogic.Instance.WarriorKilled ();
            BulletManager.Instance.RecycleBullet (col.gameObject.GetComponent<Bullet>());
        }
    }

    public bool IsAlive() {
        return !dead;
    }
}

﻿using System.Collections;
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
	private Level level;

    public GameObject container;
    public bool inQueue;

    private bool sawPlayer = false;
    private System.DateTime lastSeen;
    private static int SOUND_SPACE = 5000;

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

	public void SetLevel(Level level) {
		this.level = level;
	}

	public Level GetLevel() {
		return this.level;
	}

	// Update is called once per frame
	void Update () {
        if (dead) {
			if ((System.DateTime.Now - died).TotalMilliseconds > GameLogic.WARRIOR_SHOW_BODY) {
                warriorManager.RecycleWarrior(this);
            }
            return;
        }
		float maxDistance = Mathf.Max (CrazyCaveLevelManager.Instance.levelXSize * 2, CrazyCaveLevelManager.Instance.levelYSize * 2) * Drawer.Instance.tileLength;
		if (Vector2.Distance (transform.position, CrazyCaveGameManager.Instance.player.transform.position) > maxDistance) {
			warriorManager.RecycleWarrior (this);
			return;
		}

        if (GameLogic.Instance.IsPaused ()) {
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

			lastSeen = System.DateTime.Now;
			if (!sawPlayer) {
				sawPlayer = true;
				if (UnityEngine.Random.value < 0.15) {
					SoundManager.PlaySound ((int)SndIdGame.ZOMBIE_SEES_YOU);
				}
			}

			Vector2 velocity = this.rb.velocity;
			float angle = Mathf.Atan2 (velocity.y, velocity.x);
			transform.localRotation = Quaternion.Euler (0.0f, 0.0f, Mathf.Rad2Deg * angle - 90);
			animator.SetBool ("walking", true);
			rb.AddForce(direction.normalized * GameLogic.Instance.WarriorVelocity());
			checkOutside ();
			return;
		}
		if ((System.DateTime.Now - lastSeen).TotalMilliseconds > SOUND_SPACE) {
			sawPlayer = false;
		}
		checkOutside ();
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
            SoundManager.PlaySound ((int)SndIdGame.ZOMBIE_GOT_HIT);
        }

		if (col.gameObject.name == "Player(Clone)"  && !dead) {
			Character c = col.gameObject.GetComponent<Character> ();
			if (c.Melee) {
				dead = true;
				animator.SetBool ("dead", true);
				animator.SetBool ("walking", false);
				animator.SetBool ("attacking", false);
				died = System.DateTime.Now;
				GetComponent<Collider2D> ().enabled = false;
				GameLogic.Instance.WarriorKilled ();
				SoundManager.PlaySound ((int)SndIdGame.ZOMBIE_GOT_HIT);
				c.LifeSteal ();
			}
		}
    }
		
	void checkOutside() {
		Level.Direction direction = (Level.Direction) (-1);

		if (transform.localPosition.x > Drawer.Instance.MaxVisibleX(level)) {
			direction = Level.Direction.West;
		}
		if (transform.localPosition.x < 0) {
			direction = Level.Direction.East;
		}
		if (transform.localPosition.y > Drawer.Instance.MaxVisibleY(level)) {
			direction = Level.Direction.South;
		}
		if (transform.localPosition.y < 0) {
			direction = Level.Direction.North;
		}

		if (direction != (Level.Direction) (-1)) {
			this.transform.SetParent(CrazyCaveLevelManager.Instance.GetHolder (Level.Direction.Center).transform);
			this.level.RemoveWarrior (this);
			this.level = CrazyCaveLevelManager.Instance.GetLevel (Level.Direction.Center);
			this.level.AddWarrior (this);
		}
	}
    public bool IsAlive() {
        return !dead;
    }
}

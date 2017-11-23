using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {
	Rigidbody2D rb;
	Animator gunAnimator;
	Animator knifeAnimator;
	public GameObject ShotFireRenderer;
	[HideInInspector]
    public int health;
	[HideInInspector]
	public int bullets;
	[HideInInspector]
	public int score;
    [HideInInspector]
    public bool paused = false;
	public bool Melee = false;
	System.DateTime lastMeleeTime;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		gunAnimator = transform.Find("ShotFire").GetComponent<Animator> ();
		knifeAnimator = GetComponent<Animator> ();
		health = GameLogic.PLAYERHEALTH;
        bullets = GameLogic.STARTING_BULLETS_AMOUNT;
		score = 0;
    }

	void Update () {
		checkInput ();
	}

	private void checkInput() {
        if (paused) {
            if (Input.GetKey (KeyCode.Escape)) {
                Drawer.Instance.UnpauseText();
                paused = false;
            }
            return;
        }

        if (health > 0) {
            if (Input.GetKey (KeyCode.UpArrow)) {
                applyImpulseForward ();
            }
            if (Input.GetKey (KeyCode.DownArrow)) {
                applyImpulseBackwards ();
            }
            if (Input.GetKey (KeyCode.LeftArrow)) {
                float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * GameLogic.Instance.GetCharacterRotationSpeed ();
                transform.localRotation = Quaternion.Euler (0.0f, 0.0f, angle);
            }
            if (Input.GetKey (KeyCode.RightArrow)) {
                float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * GameLogic.Instance.GetCharacterRotationSpeed ();
                transform.localRotation = Quaternion.Euler (0.0f, 0.0f, angle);
            }
            if (Input.GetKey (KeyCode.Space)) {
                Shoot ();
            }
			if (Input.GetKey (KeyCode.LeftControl)) {
				System.DateTime now = System.DateTime.Now;
				System.TimeSpan ts = now - lastMeleeTime;
				if (ts.TotalMilliseconds > GameLogic.TIME_BETWEEN_MELEE) {
					lastMeleeTime = System.DateTime.Now;
					knifeAnimator.SetTrigger ("melee");
					Melee = true;
				} else {
					Melee = false;
				}
			} else {
				Melee = false;
			}
            if (Input.GetKey (KeyCode.Escape)) {
                Drawer.Instance.PauseText ();
                paused = true;
            }
        } else {
            if (Input.GetKey (KeyCode.Return) || Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.Escape)) {
				HighscoreController.Instance.setLastScore (this.score);
                SceneManager.LoadScene(2);
            }
        }
	}

	private Vector2 direction() {
		float angle = transform.localRotation.eulerAngles.z;
		float x = Mathf.Cos(angle * Mathf.Deg2Rad);
		float y = Mathf.Sin(angle * Mathf.Deg2Rad);
		return new Vector2 (x, y);
	}

	private void applyImpulseForward() {
		Vector2 forward = direction();
		rb.AddForce(forward * GameLogic.Instance.GetCharacterVelocity());
	}

	private void applyImpulseBackwards() {
		Vector2 forward = direction();
		rb.AddForce(-1 * forward * GameLogic.Instance.GetCharacterVelocity());
	}

	private void Shoot() {
		if (bullets > 0) {
			if (BulletManager.Instance.CanShoot()) {
				gunAnimator.SetTrigger("shootTrigger");
				BulletManager.Instance.Shoot (transform.GetChild (0).position, transform.eulerAngles, direction ());
				SoundManager.PlaySound ((int)SndIdGame.SHOT);
				bullets--;
			}
		} else {
			SoundManager.PlaySound ((int)SndIdGame.NO_AMMO);
		}
	}

		
	void OnTriggerEnter2D(Collider2D col) {
		int change;
		switch (col.gameObject.name) {
		case "Ammo(Clone)":
			change = AddBullet ();
			Drawer.Instance.CreateActionText (change.ToString(), Drawer.DARK_BLUE, col.gameObject.transform.position);
			Destroy (col.gameObject);
			break;
		case "HealthKit(Clone)":
			change = AddHealth (GameLogic.HEALTH_KIT_HP);
			Drawer.Instance.CreateActionText (change.ToString(), Drawer.GREEN, col.gameObject.transform.position);
			Destroy (col.gameObject);
			SoundManager.PlaySound ((int)SndIdGame.HEALTH_TAKEN);
			break;
		case "SpecialBox(Clone)":
			change = AddScore (GameLogic.SPECIAL_BOX_SCORE);
			Drawer.Instance.CreateActionText (change.ToString(), Drawer.PURPLE, col.gameObject.transform.position);
			Destroy (col.gameObject);
			SoundManager.PlaySound ((int)SndIdGame.SPECIAL_BOX_TAKEN);
			break;
		}
	}


	void OnCollisionEnter2D(Collision2D col) {
		int change;
		switch (col.gameObject.name) {
		case "Zombie":
			if (!Melee) {
				TakeDamage ();
				SoundManager.PlaySound ((int)SndIdGame.ZOMBIE_BITE);
			}
			break;
		}
	}

    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.name == "Zombie") {
			if (!Melee) {
				TakeDamage ();
			}
        }
    }

    private void TakeDamage(){
        if (health > 0) {
            this.health--;
            Drawer.Instance.TookDamage ();
        }
    }

	private int AddBullet() {
		if (bullets < GameLogic.MAX_AMMO) {
            int newBullets = bullets + GameLogic.AMMO_RELOAD;
			SoundManager.PlaySound ((int)SndIdGame.AMMO_PICK);
            bullets = Mathf.Min(newBullets, GameLogic.MAX_AMMO);

            if (newBullets > GameLogic.MAX_AMMO) {
                return (int)Mathf.Abs(GameLogic.AMMO_RELOAD - (newBullets - GameLogic.MAX_AMMO));
            } else {
                return GameLogic.AMMO_RELOAD;
            }
		}
		return 0;
	}

	private int AddHealth(int amount) {
		int newHealth = health + amount;
		if (newHealth > GameLogic.PLAYERHEALTH) {
			health = GameLogic.PLAYERHEALTH;
			int reminder = newHealth - GameLogic.PLAYERHEALTH;
			return Mathf.Abs (reminder - amount);
		} else {
			health = newHealth;
			return amount;
		}
	}

	private int AddScore(int amount) {
		score += amount;
		return amount;
	}

	public void ZombieKilled() {
		AddScore (GameLogic.ZOMBIE_KILLED_SCORE);
	}
}

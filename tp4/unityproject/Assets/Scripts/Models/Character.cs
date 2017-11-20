using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {
	Rigidbody2D rb;
	Animator animator;
	public GameObject ShotFireRenderer;
	[HideInInspector]
    public int health;
	[HideInInspector]
	public int bullets;
	[HideInInspector]
	public int score;

	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponentInChildren<Animator> ();
		health = GameLogic.PLAYERHEALTH;
		bullets = GameLogic.MAX_AMMO;
		score = 0;
    }

	void Update () {
		checkInput ();
	}

	private void checkInput() {
		if (Input.GetKey(KeyCode.UpArrow)) {
			applyImpulseForward();
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			applyImpulseBackwards ();
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * GameLogic.Instance.GetCharacterRotationSpeed();
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}
		if (Input.GetKey (KeyCode.RightArrow)) {
			float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * GameLogic.Instance.GetCharacterRotationSpeed ();
			transform.localRotation = Quaternion.Euler (0.0f, 0.0f, angle);
		}
		if (Input.GetKey (KeyCode.Space)) {
			Shoot ();
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
			if (BulletManager.Instance.Shoot (transform.GetChild (0).position, transform.eulerAngles, direction ())) {
//				animator.SetTrigger("ShootTrigger");
				SoundManager.PlaySound ((int)SndIdGame.SHOT);
				bullets--;
			}
		} else {
			SoundManager.PlaySound ((int)SndIdGame.NO_AMMO);
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		switch (col.gameObject.name) {
			case "ZombiePrefab":
				TakeDamage ();
				break;
			case "Ammo(Clone)":
				AddBullet ();
				Destroy (col.gameObject);
				break;
			case "HealthKit(Clone)":
				AddHealth (GameLogic.HEALTH_KIT_HP);
				Destroy (col.gameObject);
				break;
			case "SpecialBox(Clone)":
				AddScore (GameLogic.SPECIAL_BOX_SCORE);
				Destroy (col.gameObject);
				break;
		}
	}

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.name == "ZombiePrefab") {
            TakeDamage();
        }
    }

    private void TakeDamage(){
        this.health--;
        if (health <= 0) {
//            HighscoreController.instance.setLastScore(logic.Score());
//            SceneManager.LoadScene(2);
        }
    }

	private void AddBullet() {
		if (bullets < GameLogic.MAX_AMMO) {
			SoundManager.PlaySound ((int)SndIdGame.AMMO_PICK);
			bullets++;
		}
	}

	private void AddHealth(int amount) {
		int newHealth = health + amount;
		health = Mathf.Max (amount, GameLogic.PLAYERHEALTH);
	}

	private void AddScore(int amount) {
		score += amount;
	}

	public void ZombieKilled() {
		AddScore (GameLogic.ZOMBIE_KILLED_SCORE);
	}
}

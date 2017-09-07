using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {

	Rigidbody2D rb;
	Animator animator;
	public GameObject ShotFireRenderer;
	public BulletManager bulletManager;
	public GameLogic logic;
    public int health;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponentInChildren<Animator> ();
        health = GameLogic.PLAYERHEALTH;
    }

	void Awake() {
    }

	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	void FixedUpdate() {
		transform.position = PositionManager.Reposition (transform.position);
	}

	private void checkInput() {
		if (Input.GetKey(KeyCode.UpArrow)) {
			applyImpulse();
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * logic.GetCharacterRotationSpeed();
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * logic.GetCharacterRotationSpeed();
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}
		if (Input.GetKey (KeyCode.Space)) {
			if (bulletManager.Shoot (transform.GetChild(0).position, transform.eulerAngles, direction()))
            {
                animator.SetTrigger("ShootTrigger");
            }
		}
	}

	private Vector2 direction()
	{
		float angle = transform.localRotation.eulerAngles.z;
		float x = Mathf.Cos(angle * Mathf.Deg2Rad);
		float y = Mathf.Sin(angle * Mathf.Deg2Rad);
		return new Vector2 (x, y);
	}

	private void applyImpulse()
	{
		Vector2 forward = direction();
		rb.AddForce(forward * logic.GetCharacterVelocity());
	}

	void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.name == "ZombiePrefab") {
            takeDamage();
        }
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "ZombiePrefab")
        {
            takeDamage();
        }
    }

    void takeDamage(){
        this.health--;
        if (health <= 0)
        {
            HighscoreController.instance.setLastScore(logic.Score());
            SceneManager.LoadScene(2);
        }
    }
}

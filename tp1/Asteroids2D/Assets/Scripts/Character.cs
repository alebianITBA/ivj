using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	Rigidbody2D rb;
	Animator animator;
	public GameObject ShotFireRenderer;
	public BulletManager bulletManager;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		animator = GetComponentInChildren<Animator> ();
		ShotFireRenderer.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		checkInput ();
	}

	private void checkInput() {
		if (Input.GetKey(KeyCode.UpArrow)) {
			applyImpulse();
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * GameLogic.CHARACTER_ROTATION_SPEED;
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * GameLogic.CHARACTER_ROTATION_SPEED;
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}
		if (Input.GetKey (KeyCode.Space)) {
			ShotFireRenderer.SetActive (true);
			animator.SetTrigger("ShootTrigger");
			bulletManager.Shoot (transform.GetChild(0).position, transform.eulerAngles, direction());
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
		rb.AddForce(forward * GameLogic.CHARACTER_VELOCITY);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

	int ROTATION_SPEED = 200;
	float VELOCITY = 30.0f;

	Rigidbody2D rb;
	Animator animator;
	public GameObject ShotFireRenderer;

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
			float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * ROTATION_SPEED;
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * ROTATION_SPEED;
			transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
		}
		if (Input.GetKey (KeyCode.Space)) {
			ShotFireRenderer.SetActive (true);
			animator.SetTrigger("ShootTrigger");
		}
	}

	private void applyImpulse()
	{
		float angle = transform.localRotation.eulerAngles.z;
		float x = Mathf.Cos(angle * Mathf.Deg2Rad);
		float y = Mathf.Sin(angle * Mathf.Deg2Rad);
		Vector2 forward = new Vector2(x, y);
		rb.AddForce(forward * VELOCITY);
	}
}

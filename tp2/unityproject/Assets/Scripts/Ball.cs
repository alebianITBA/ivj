using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public enum BallTypes { White, Black, Striped, Solid };
	public string id;
	public BallTypes type;

	void Update() {
		var rigidbody = GetComponent<Rigidbody>();
		if (rigidbody.velocity.y > 0) {
			var velocity = rigidbody.velocity;
			velocity.y *= 0.3f;
			rigidbody.velocity = velocity;
		}
	}
}

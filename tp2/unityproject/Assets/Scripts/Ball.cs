using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public enum BallTypes { None, White, Black, Striped, Solid };
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

	public static BallTypes Opposite(BallTypes type) {
		switch(type) {
			case BallTypes.Solid:
				return BallTypes.Striped;
			case BallTypes.Striped:
				return BallTypes.Solid;
			case BallTypes.White:
				return BallTypes.Black;
			case BallTypes.Black:
				return BallTypes.White;
			default:
				return BallTypes.None;
		}
	}
}

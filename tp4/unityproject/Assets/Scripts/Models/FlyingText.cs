using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingText : MonoBehaviour {
	private static float MOVEMENT = 0.05f;

	void Update () {
		Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + MOVEMENT, -10);
		transform.position = newPosition;
	}
}

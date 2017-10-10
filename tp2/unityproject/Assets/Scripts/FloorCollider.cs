using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCollider : MonoBehaviour {
	void OnCollisionEnter(Collision col) {
		Ball ball = (Ball)col.gameObject.GetComponent<Ball> ();
		StateManager.Instance.PutBallBackInTable (col.gameObject);
	}
}
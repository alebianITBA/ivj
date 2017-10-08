using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketCollider : MonoBehaviour {
	public enum Pocket { TopLeft, TopRight, MiddleLeft, MiddleRight, BottomLeft, BottomRight };
	public Pocket pocketId;

	void OnCollisionEnter(Collision col) {
		StateManager.Instance.BallEnteredInPocket (col.gameObject, pocketId);
	}
}

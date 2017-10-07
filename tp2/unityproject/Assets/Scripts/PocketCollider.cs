using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketCollider : MonoBehaviour {
	public enum Pocket { TopLeft, TopRight, MiddleLeft, MiddleRight, BottomLeft, BottomRight };
	public Pocket pocketId;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision col) {
		print ("Collided " + col.gameObject.name);
		StateManager.Instance.BallEnteredInPocket (col.gameObject, pocketId);
	}
}

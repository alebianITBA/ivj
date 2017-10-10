using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBall : Ball {

	private BallTypes firstCollided;
	// Use this for initialization
	void Start () {
		this.type = BallTypes.White;
		this.firstCollided = BallTypes.None;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		BasicSoundManager.Instance.PlayBallHitSound ();
		Ball b = collision.gameObject.GetComponent<Ball> ();
		if (b != null && this.firstCollided == BallTypes.None) {
			this.firstCollided = b.type;
		}
	}

	public void ResetFirstCollided() {
		this.firstCollided = BallTypes.None;
	}

	public BallTypes GetFirstCollided() {
		return this.firstCollided;
	}
}

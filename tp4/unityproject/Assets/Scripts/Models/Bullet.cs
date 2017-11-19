using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public System.DateTime ShootedAt;
//	BulletManager bulletManager;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if ((System.DateTime.Now - ShootedAt).TotalMilliseconds > GameLogic.BULLET_TTL) {
//			bulletManager.RecycleBullet (this);
		}
	}

//	public void SetManager(BulletManager manager) {
//		bulletManager = manager;
//	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "ZombiePrefab") {
//			bulletManager.RecycleBullet(this);
		}
	}
}

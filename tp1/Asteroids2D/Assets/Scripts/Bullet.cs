using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public System.DateTime ShootedAt;
	BulletManager bulletManager;
	float TTL = BulletManager.TIME_BETWEEN_SHOTS * (BulletManager.BULLET_LIMIT - 1);

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if ((System.DateTime.Now - ShootedAt).TotalMilliseconds > TTL) {
			bulletManager.RecycleBullet (this);
		}
	}

	public void SetManager(BulletManager manager) {
		bulletManager = manager;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public System.DateTime ShootedAt;
	BulletManager bulletManager;

	void Update () {
		if ((System.DateTime.Now - ShootedAt).TotalMilliseconds > GameLogic.BULLET_TTL) {
			bulletManager.RecycleBullet (this);
		}
	}

	public void SetManager(BulletManager manager) {
		bulletManager = manager;
	}
}

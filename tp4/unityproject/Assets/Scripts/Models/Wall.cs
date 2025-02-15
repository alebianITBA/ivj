﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.name == "Bullet") {
			BulletManager.Instance.RecycleBullet(col.gameObject.GetComponent<Bullet>());
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviourSingleton<BulletManager> {

	Queue<Bullet> bulletPool;
	public GameObject bulletPrefab;

	System.DateTime lastShootTime;
	int shootNumber = 0;

	// Use this for initialization
	void Start () {
		PrecreateObjects ();
	}

	private void PrecreateObjects() {
		bulletPool = new Queue<Bullet>();
		for (int i = 0; i < GameLogic.TOTAL_BULLETS_AMOUNT; i++) {
			GameObject go = GameObject.Instantiate(bulletPrefab) as GameObject;
			Bullet bul = go.GetComponent<Bullet>();
			bul.SetManager (this);
			if (bul == null) {
				Debug.LogError ("Cannot fint the component Bullet in the bullet prefab.");
			}
			go.name = bulletPrefab.name;
			go.transform.parent = transform;
			go.SetActive(false);
			bulletPool.Enqueue(bul);
		}
	}

	public bool Shoot(Vector2 pos, Vector3 rot, Vector2 dir) {
		System.DateTime now = System.DateTime.Now;
		System.TimeSpan ts = now - lastShootTime;
		if (ts.TotalMilliseconds > GameLogic.TIME_BETWEEN_SHOTS && bulletPool.Count > 0) {
			shootNumber++;
			lastShootTime = System.DateTime.Now;
			Bullet bul = bulletPool.Dequeue();

			bul.ShootedAt = now;
			bul.transform.rotation = transform.rotation;
			bul.transform.position = pos;
			bul.transform.Rotate (0, 0, rot.z - 90);
			bul.gameObject.SetActive(true);

			bul.GetComponent<Rigidbody2D> ().AddForce (dir * GameLogic.BULLET_SPEED);
            return true;
		}
        return false;
	}

	public void RecycleBullet(Bullet bul) {
		bulletPool.Enqueue(bul);
		bul.gameObject.SetActive(false);
	}

	public void IgnoreColliders(Collider2D collider) {
		foreach(Bullet b in bulletPool) {
			Physics2D.IgnoreCollision (collider, b.GetComponent<Collider2D>());
		}
	}
}

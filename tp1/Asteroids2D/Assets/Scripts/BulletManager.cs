using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

	int BULLET_LIMIT = 10;
	float BULLET_SPEED = 1000.0f;
	float TIME_BETWEEN_SHOTS = 100.0f;

	Queue<Bullet> bulletPool;
	public GameObject bulletPrefab;

	System.DateTime lastShootTime;
	int shootNumber = 0;

	// Use this for initialization
	void Start () {
		PrecreateObjects ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void PrecreateObjects()
	{
		bulletPool = new Queue<Bullet>();
		for (int i = 0; i < BULLET_LIMIT; i++)
		{
			GameObject go = GameObject.Instantiate(bulletPrefab) as GameObject;
			Bullet bul = go.GetComponent<Bullet>();
			if (bul == null) {
				Debug.LogError ("Cannot fint the component Bullet in the bullet prefab.");
			}
			go.name = bulletPrefab.name;
			go.transform.parent = transform;
			go.SetActive(false);
			bulletPool.Enqueue(bul);
		}
	}

	public void Shoot(Vector2 pos, Vector3 rot, Vector2 dir)
	{
		System.TimeSpan ts = System.DateTime.Now - lastShootTime;
		if (ts.TotalMilliseconds > TIME_BETWEEN_SHOTS && bulletPool.Count > 0)
		{
			shootNumber++;
			lastShootTime = System.DateTime.Now;
			Bullet bul = bulletPool.Dequeue();
			bul.transform.position = pos;
			bul.transform.Rotate (0, 0, rot.z - 90);
			bul.gameObject.SetActive(true);
			bul.GetComponent<Rigidbody2D> ().AddForce (dir * BULLET_SPEED);
		}
	}

	public void RecycleBullet(Bullet bul)
	{
		bulletPool.Enqueue(bul);
		bul.gameObject.SetActive(false);
	}
}

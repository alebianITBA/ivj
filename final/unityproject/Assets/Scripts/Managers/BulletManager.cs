using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    private float bulletDamage;
    private Transform parentTransform;
    private GameObject bulletPrefab;
    private GameManager.Teams team;
    private Queue<Bullet> bulletPool;

    public BulletManager (int amount, float bulletDamage, GameObject bulletPrefab, Transform parentTransform, GameManager.Teams team)
    {
        this.bulletDamage = bulletDamage;
        this.bulletPrefab = bulletPrefab;
        this.parentTransform = parentTransform;
        this.team = team;
        PrecreateObjects(amount);
    }

    private void PrecreateObjects (int amount)
    {
        bulletPool = new Queue<Bullet>();
        for (int i = 0; i < amount; i++) {
            GameObject go = GameObject.Instantiate(bulletPrefab) as GameObject;
            Bullet bul = go.GetComponent<Bullet>();
            bul.SetManager(this);
            bul.SetDamage(this.bulletDamage);
            bul.SetTeam(team);
            if (bul == null) {
                Debug.LogError("Cannot fint the component Bullet in the bullet prefab.");
            }
            go.name = "Bullet";
//            go.transform.parent = parentTransform;
            go.SetActive(false);
            bulletPool.Enqueue(bul);
        }
    }

    public void Shoot (Vector2 pos, Vector3 rot, Vector2 dir, Quaternion rotation)
    {
        Bullet bul = bulletPool.Dequeue();
        bul.ShootedAt = System.DateTime.Now;
        bul.transform.rotation = rotation;
        bul.transform.position = pos;
        bul.transform.Rotate(0, 0, rot.z - 90);
        bul.gameObject.SetActive(true);
        bul.GetComponent<Rigidbody2D>().AddForce(dir * Constants.BULLET_SPEED);
    }

    public void RecycleBullet (Bullet bul)
    {
        bulletPool.Enqueue(bul);
        bul.gameObject.SetActive(false);
    }

    public void IgnoreColliders (Collider2D collider)
    {
        if (bulletPool == null) {
            return;
        }

        foreach (Bullet b in bulletPool) {
            Physics2D.IgnoreCollision(collider, b.GetComponent<Collider2D>());
        }
    }

    public int BulletsLeft ()
    {
        return bulletPool.Count;
    }
}

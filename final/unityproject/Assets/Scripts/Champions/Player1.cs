using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : Champion
{
    private static float TIME_BETWEEN_SHOTS = 300.0f;
    private static int BULLETS_PER_SHOT = 3;
    private static float BULLET_DAMAGE = 9.0f;
    private System.DateTime lastShootTime;

    public GameObject bulletPrefab;
    private BulletManager bulletManager;
    public GameObject shootPointer;

    void Start ()
    {
        this.health = Constants.PLAYER_MAX_BASE_HEALTH;
        rb = GetComponent<Rigidbody2D>();
        int amount = (int)(Mathf.Ceil(1000.0f / TIME_BETWEEN_SHOTS) * BULLETS_PER_SHOT * 2.0f);
        this.bulletManager = new BulletManager(amount, BULLET_DAMAGE, bulletPrefab, GetTeam());
        bulletManager.IgnoreColliders(GetComponent<Collider2D>());
    }

    void Update ()
    {
        if (this.cam != null) {
            this.cam.transform.position = new Vector3(transform.position.x, transform.position.y, -100);
            this.cam.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        checkInput();
    }

    protected void checkInput ()
    {
        if (GetCurrentHealth() > 0) {
            if (Input.GetKey(KeyCode.W)) {
                applyImpulseForward();
            }
            if (Input.GetKey(KeyCode.S)) {
                applyImpulseBackwards();
            }
            if (Input.GetKey(KeyCode.A)) {
                float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * Constants.PLAYER_BASE_ROTATION_SPEED;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }
            if (Input.GetKey(KeyCode.D)) {
                float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * Constants.PLAYER_BASE_ROTATION_SPEED;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }
            if (Input.GetKey(KeyCode.Space)) {
                Shoot();
            }
        }
    }

    private void Shoot ()
    {
        if (CanShoot()) {
            lastShootTime = System.DateTime.Now;
            bulletManager.Shoot(shootPointer.transform.position, transform.eulerAngles, direction(), transform.rotation);
            bulletManager.Shoot(shootPointer.transform.position, transform.eulerAngles, direction(15.0f), transform.rotation);
            bulletManager.Shoot(shootPointer.transform.position, transform.eulerAngles, direction(-15.0f), transform.rotation);
//            GameObject go = GameObject.Instantiate(bulletPrefab) as GameObject;
//            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), go.GetComponent<Collider2D>());
//            Bullet bul = go.GetComponent<Bullet>();
//            bul.SetDamage(BULLET_DAMAGE);
//            bul.SetTeam(team);
//            go.name = "Bullet";
//            go.SetActive(true);
//            bul.ShootedAt = System.DateTime.Now;
//            bul.transform.rotation = transform.rotation;
//            bul.transform.position = transform.position;
//            bul.transform.Rotate(0, 0, transform.eulerAngles.z - 90);
//            bul.gameObject.SetActive(true);
//            bul.GetComponent<Rigidbody2D>().AddForce(direction() * Constants.BULLET_SPEED);
        }
    }

    private bool CanShoot ()
    {
        System.DateTime now = System.DateTime.Now;
        System.TimeSpan ts = now - lastShootTime;
        return ts.TotalMilliseconds > TIME_BETWEEN_SHOTS && bulletManager.BulletsLeft() >= BULLETS_PER_SHOT;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : Champion
{
    private static float TIME_BETWEEN_SHOTS = 100.0f;
    private static int BULLETS_PER_SHOT = 1;
    private static float BULLET_DAMAGE = 3.0f;
    public GameObject bulletPrefab;
    private BulletManager bulletManager;
    private System.DateTime lastShootTime;
    public GameObject shootPointer;

    void Start ()
    {
        this.health = Constants.PLAYER_MAX_BASE_HEALTH;
        rb = GetComponent<Rigidbody2D>();
        int amount = (int)(Mathf.Ceil(1000.0f / TIME_BETWEEN_SHOTS) * BULLETS_PER_SHOT * 2.0f);
        this.bulletManager = new BulletManager(amount, BULLET_DAMAGE, bulletPrefab, GetTeam());
        bulletManager.IgnoreColliders(GetComponent<PolygonCollider2D>());
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
            if (Input.GetKey(KeyCode.UpArrow)) {
                applyImpulseForward();
            }
            if (Input.GetKey(KeyCode.DownArrow)) {
                applyImpulseBackwards();
            }
            if (Input.GetKey(KeyCode.LeftArrow)) {
                float angle = transform.localRotation.eulerAngles.z + Time.deltaTime * Constants.PLAYER_BASE_ROTATION_SPEED;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }
            if (Input.GetKey(KeyCode.RightArrow)) {
                float angle = transform.localRotation.eulerAngles.z - Time.deltaTime * Constants.PLAYER_BASE_ROTATION_SPEED;
                transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            }
            if (Input.GetKey(KeyCode.L)) {
                Shoot();
            }
        }
    }

    private void Shoot ()
    {
        if (CanShoot()) {
            lastShootTime = System.DateTime.Now;
            bulletManager.Shoot(shootPointer.transform.position, transform.eulerAngles, direction(), transform.rotation);
        }
    }

    private bool CanShoot ()
    {
        System.DateTime now = System.DateTime.Now;
        System.TimeSpan ts = now - lastShootTime;
        return ts.TotalMilliseconds > TIME_BETWEEN_SHOTS && bulletManager.BulletsLeft() >= BULLETS_PER_SHOT;
    }
}

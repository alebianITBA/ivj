using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour, Life<Tower>, Team
{
    private float health;
    public GameObject healthBar;
    public GameManager.Teams team;
    public bool alive;
    public GameObject shootPointer;
    public GameObject rocketPrefab;
    private RocketManager rocketManager;
    private System.DateTime lastShootTime;

    private static float TIME_BETWEEN_SHOTS = 1000.0f;

    // SPRITES
    public Sprite leftSprite;
    public Sprite leftDownSprite;
    public Sprite leftUpSprite;
    public Sprite rightSprite;
    public Sprite rightDownSprite;
    public Sprite rightUpSprite;
    public Sprite upSprite;
    public Sprite downSprite;

    void Start ()
    {
        this.health = Constants.TOWER_MAX_BASE_HEALTH;
        this.alive = true;
        this.rocketManager = new RocketManager(5, rocketPrefab, GetTeam());
        rocketManager.IgnoreColliders(gameObject.GetComponent<PolygonCollider2D>());
    }

    void Update ()
    {
        if (healthBar != null) {
            if (health > 0) {
                healthBar.transform.localScale = new Vector3(GetCurrentHealth() / GetTotalHealth(), 1.0f, 1.0f);
            }
            else {
                healthBar.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                healthBar.GetComponent<Image>().color = Color.gray;
            }
        }
    }

    void FixedUpdate ()
    {
        if (alive) {
            TryToShoot();
        }
    }

    public float Heal (float amount)
    {
        return 0.0f;
    }

    public float TakeDamage (float amount)
    {
        if (health > 0) {
            this.health -= amount;
            return amount;
        }
        else {
            this.alive = false;
            return 0.0f;
        }
    }

    public float GetTotalHealth ()
    {
        return Constants.TOWER_MAX_BASE_HEALTH;
    }

    public float GetCurrentHealth ()
    {
        return this.health;
    }

    public bool IsRED ()
    {
        return this.team == GameManager.Teams.RED;
    }

    public bool IsBLUE ()
    {
        return this.team == GameManager.Teams.BLUE;
    }

    public GameManager.Teams GetTeam ()
    {
        return this.team;
    }

    public void SetTeam (GameManager.Teams team)
    {
        this.team = team;
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.name == "Bullet") {
            Bullet bul = col.gameObject.GetComponent<Bullet>();
            bul.Recycle();
            if (bul.GetTeam() != GetTeam()) {
                TakeDamage(bul.GetDamage() * 5);
            }
        }
    }

    private void TryToShoot ()
    {
        if (CanShoot()) {
            if (IsBLUE()) {
                foreach (GameObject player in GameManager.Instance.REDPlayers) {
                    if (CloseEnoughToShoot(player)) {
                        Shoot(player);
                        return;
                    }
                }
                foreach (GameObject minion in GameManager.Instance.REDMinions) {
                    if (CloseEnoughToShoot(minion)) {
                        Shoot(minion);
                        return;
                    }
                }
            }
            else {
                foreach (GameObject player in GameManager.Instance.BLUEPlayers) {
                    if (CloseEnoughToShoot(player)) {
                        Shoot(player);
                        return;
                    }
                    else {
                    }
                }
                foreach (GameObject minion in GameManager.Instance.BLUEMinions) {
                    if (CloseEnoughToShoot(minion)) {
                        Shoot(minion);
                        return;
                    }
                }
            }
        }
    }

    private void Shoot (GameObject obj)
    {
        Vector2 dir = direction(obj);
        rocketManager.Shoot(shootPointer.transform.position, transform.eulerAngles, dir, transform.rotation);
        lastShootTime = System.DateTime.Now;
        changeSprite(obj);
    }

    private void changeSprite (GameObject obj)
    {
        Vector2 dir = obj.transform.position - shootPointer.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle >= -22.5f && angle < 22.5f) {
            GetComponent<SpriteRenderer>().sprite = rightSprite;
        }
        else if (angle >= 22.5f && angle < 67.5f) {
            GetComponent<SpriteRenderer>().sprite = rightUpSprite;
        }
        else if (angle >= 67.5f && angle < 112.5f) {
            GetComponent<SpriteRenderer>().sprite = upSprite;
        }
        else if (angle >= 112.5f && angle < 157.5f) {
            GetComponent<SpriteRenderer>().sprite = leftUpSprite;
        }
        else if (angle >= 157.5f && angle < -157.5f) {
            GetComponent<SpriteRenderer>().sprite = leftSprite;
        }
        else if (angle >= -157.5f && angle < -112.5f) {
            GetComponent<SpriteRenderer>().sprite = leftDownSprite;
        }
        else if (angle >= -112.5f && angle < -67.5f) {
            GetComponent<SpriteRenderer>().sprite = downSprite;
        }
        else if (angle >= -67.5f && angle < -22.5f) {
            GetComponent<SpriteRenderer>().sprite = rightDownSprite;
        }
    }

    private bool CanShoot ()
    {
        System.DateTime now = System.DateTime.Now;
        System.TimeSpan ts = now - lastShootTime;
        return ts.TotalMilliseconds > TIME_BETWEEN_SHOTS && rocketManager.RocketsLeft() > 0;
    }

    private bool CloseEnoughToShoot (GameObject obj)
    {
        float distance = Vector2.Distance(obj.transform.position, shootPointer.transform.position);
        return distance <= 0.72f;
    }

    private Vector2 direction (GameObject target)
    {
        return target.transform.position - shootPointer.transform.position;
    }
}

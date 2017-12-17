using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, Team
{
    public System.DateTime ShootedAt;
    [HideInInspector]
    public BulletManager bulletManager;
    private float damage;
    private GameManager.Teams team;

    void Update ()
    {
        if ((System.DateTime.Now - ShootedAt).TotalMilliseconds > 2000.0f) {
            Recycle();
        }
    }

    public void SetManager (BulletManager manager)
    {
        bulletManager = manager;
    }

    public void Recycle ()
    {
        bulletManager.RecycleBullet(this);
    }

    public void SetDamage (float damage)
    {
        this.damage = damage;
    }

    public float GetDamage ()
    {
        return this.damage;
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
}


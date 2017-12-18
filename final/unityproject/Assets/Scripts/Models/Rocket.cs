using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, Team
{
    public System.DateTime ShootedAt;
    private GameManager.Teams team;
    private RocketManager rocketManager;

    public void SetManager (RocketManager manager)
    {
        rocketManager = manager;
    }

    public void Recycle ()
    {
        rocketManager.RecycleRocket(this);
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

    public float GetDamage ()
    {
        return 20.0f;
    }
}

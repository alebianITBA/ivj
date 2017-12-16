using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawn : MonoBehaviour, Team
{
    private bool active;
    public Sprite activeSprite;
    public Sprite deactiveSprite;
    public GameManager.Teams team;

    void Start ()
    {
        this.active = false;
    }

    void Update ()
    {
    }

    public void Activate ()
    {
        this.active = true;
        GetComponent<SpriteRenderer>().sprite = activeSprite;
    }

    public void Deactivate ()
    {
        this.active = false;
        GetComponent<SpriteRenderer>().sprite = deactiveSprite;
    }

    public void ReleaseMinions ()
    {
        if (active) {
            // TODO: add minion manager and release minions
        }
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

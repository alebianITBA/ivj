using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public enum Teams
    {
        RED,
        BLUE}

    ;

    public TextAsset mapTextFile;
    private Level level;

    void Start ()
    {
        this.level = new Level(mapTextFile);
        Drawer.Instance.SetLevel(level);
        Drawer.Instance.DrawMap();
        Drawer.Instance.DrawObjectives();
    }

    public void AssignTeam (GameObject obj, Team team)
    {
        if (obj.transform.position.x <= 2.48f) {
            team.SetTeam(GameManager.Teams.RED);
        }
        else {
            team.SetTeam(GameManager.Teams.BLUE);
        }
    }
}

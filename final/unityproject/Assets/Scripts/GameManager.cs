using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    public enum Teams
    {
        RED,
        BLUE}

    ;

    public TextAsset mapTextFile;
    private Level level;
    private bool gameEnded;

    public GameObject panel;
    public Text middleText;

    public Base BLUEBase { get; set; }

    public Base REDBase { get; set; }

    void Start ()
    {
        this.level = new Level(mapTextFile);
        this.gameEnded = false;
        Drawer.Instance.SetLevel(level);
        Drawer.Instance.DrawMap();
        Drawer.Instance.DrawObjectives();
    }

    void Update ()
    {
        if (!GameFinished()) {
            if (!REDBase.alive || !BLUEBase.alive) {
                gameEnded = true;
                ShowWINText();
            }
        }
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

    public void ShowWINText ()
    {
        middleText.text = "GAME ENDED. WINNER: " + (REDBase.alive ? "RED TEAM" : "BLUE TEAM");
        middleText.enabled = true;
    }

    public bool GameFinished ()
    {
        return gameEnded;
    }
}

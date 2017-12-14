using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
	public TextAsset mapTextFile;
	private Level level;

	void Start ()
	{
		this.level = new Level(mapTextFile);
		Drawer.Instance.SetLevel(level);
		Drawer.Instance.DrawMap();
		Drawer.Instance.DrawObjectives();
	}
}

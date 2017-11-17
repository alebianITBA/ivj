using System;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
	private Level level;

	public GameObject floorPrefab;
	public GameObject wallPrefab;

	void Start() {
		this.level = LevelManager.Instance.CreateLevel (5, 5);
		DrawTiles ();
	}

	private void DrawTiles() {
		for (int row = 0; row < level.map.GetLength(0); row++) {
			for (int col = 0; col < level.map.GetLength(1); col++) {
				GameObject go;
				Vector3 position = new Vector3 (row, col, 0);
				switch (level.map [row, col]) {
				case Level.Tile.Floor:
					go = Instantiate (floorPrefab, position, Quaternion.identity) as GameObject;	
					break;
				case Level.Tile.Wall:
					go = Instantiate (wallPrefab, position, Quaternion.identity) as GameObject;
					break;
				}
			}
		}
	}
}

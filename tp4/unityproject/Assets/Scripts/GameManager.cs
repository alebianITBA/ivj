using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
	private Level level;

	// TODO: Make this an array and access them randomly
	public GameObject[] floorPrefabs;
	public GameObject wallPrefab;

	public int levelXSize;
	public int levelYSize;
	public int automataInitialRounds;
	public int automataAfterRounds;
	public int automataInitialBirthChance;
	public int automataInitialDeathChance;
	public int automataAfterBirthChance;
	public int automataAfterDeathChance;
	public float automataInitialWallChance;

	void Start() {
		this.level = LevelManager.Instance.GetAutomataLevel (levelXSize, levelYSize, automataInitialRounds,
			automataAfterRounds, automataInitialBirthChance, automataInitialDeathChance, automataAfterBirthChance, 
			automataAfterDeathChance, automataInitialWallChance);
		DrawTiles ();
	}

	private void DrawTiles() {
		for (int row = 0; row < level.map.GetLength(0); row++) {
			for (int col = 0; col < level.map.GetLength(1); col++) {
				GameObject go;
				float len = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x;
				Vector3 position = new Vector3 (row * len, col * len, 0);
				switch (level.map [row, col]) {
				case Level.Tile.Floor:
					go = Instantiate (RandomTile(floorPrefabs), position, Quaternion.identity) as GameObject;	
					break;
				case Level.Tile.Wall:
					go = Instantiate (wallPrefab, position, Quaternion.identity) as GameObject;
					break;
				}
			}
		}
	}

	private GameObject RandomTile(GameObject[] tiles) {
		return tiles[UnityEngine.Random.Range (0, tiles.Length)];
	}
}

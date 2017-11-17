using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager> {
	private Level level;
	GameObject boardHolder;

	public GameObject[] floorPrefabs;
	public GameObject[] wallPrefabs;
	public GameObject[] outerWallPrefabs;

	public int levelXSize;
	public int levelYSize;
	public Level.LevelType levelType;
	// Automata generator attributes
	public int automataInitialRounds;
	public int automataAfterRounds;
	public int automataInitialBirthChance;
	public int automataInitialDeathChance;
	public int automataAfterBirthChance;
	public int automataAfterDeathChance;
	public float automataInitialWallChance;

	void Start() {
		this.boardHolder = new GameObject("BoardHolder");
		StartLevel ();
		DrawTiles ();
	}

	private void StartLevel() {
		switch (levelType) {
		case Level.LevelType.Automata:
			this.level = LevelManager.Instance.GetAutomataLevel (levelXSize, levelYSize, automataInitialRounds,
				automataAfterRounds, automataInitialBirthChance, automataInitialDeathChance, automataAfterBirthChance, 
				automataAfterDeathChance, automataInitialWallChance);
			break;
		}
	}

	private void DrawTiles() {
		for (int row = 0; row < level.map.GetLength(0); row++) {
			for (int col = 0; col < level.map.GetLength(1); col++) {
				GameObject tileInstance = null;
				float len = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x;
				Vector3 position = new Vector3 (row * len, col * len, 0);
				switch (level.map [row, col]) {
				case Level.Tile.Floor:
					tileInstance = Instantiate (RandomTile(floorPrefabs), position, Quaternion.identity) as GameObject;	
					break;
				case Level.Tile.Wall:
					tileInstance = Instantiate (RandomTile(wallPrefabs), position, Quaternion.identity) as GameObject;
					break;
				}
				tileInstance.transform.parent = boardHolder.transform;
			}
		}
	}

	private GameObject RandomTile(GameObject[] tiles) {
		return tiles[UnityEngine.Random.Range (0, tiles.Length)];
	}
}

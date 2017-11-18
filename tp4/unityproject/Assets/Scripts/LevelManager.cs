using UnityEngine;

public class LevelManager : MonoBehaviourSingleton<LevelManager> {
	private Level level;
	GameObject boardHolder = null;

	public GameObject[] floorPrefabs;
	public GameObject[] wallPrefabs;
	public GameObject[] outerWallPrefabs;

	public int levelXSize;
	public int levelYSize;
	// Automata generator attributes
	public int automataInitialRounds;
	public int automataAfterRounds;
	public int automataInitialBirthChance;
	public int automataInitialDeathChance;
	public int automataAfterBirthChance;
	public int automataAfterDeathChance;
	public float automataInitialWallChance;

	public void CreateNewLevel() {
		if (this.boardHolder != null) {
			// There was an existing level so we destroy it
			Destroy(boardHolder);
		}
		this.boardHolder = new GameObject ("BoardHolder");

		CreateLevel ();
		FillOuterWalls ();
		DrawTiles ();
	}

	private void CreateLevel() {
//		switch (levelType) {
//		case Level.LevelType.Automata:
//			this.level = new AutomataLevel (levelXSize, levelYSize, automataInitialRounds, automataAfterRounds, automataInitialBirthChance,
//				automataInitialDeathChance, automataAfterBirthChance, automataAfterDeathChance, automataInitialWallChance);
//			break;
//		case Level.LevelType.Room:
//			this.level = new RoomLevel (levelXSize, levelYSize);
//			break;
//		}
	}

	private void FillOuterWalls() {
		// TODO
	}

	private void DrawTiles() {
		for (int row = 0; row < level.GetMap().GetLength(0); row++) {
			for (int col = 0; col < level.GetMap().GetLength(1); col++) {
				GameObject tileInstance = null;
				float len = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x;
				Vector3 position = new Vector3 (row * len, col * len, 0);

				switch (level.GetMap() [row, col]) {
				case Level.Tile.Floor:
					tileInstance = Instantiate (RandomTile(floorPrefabs), position, Quaternion.identity) as GameObject;	
					break;
				case Level.Tile.PlayerSpawn:
					// TODO: Make a different floor sprite
					tileInstance = Instantiate (RandomTile(floorPrefabs), position, Quaternion.identity) as GameObject;	
					break;
				case Level.Tile.ZombieSpawn:
					// TODO: Make a different floor sprite
					tileInstance = Instantiate (RandomTile(floorPrefabs), position, Quaternion.identity) as GameObject;	
					break;
				case Level.Tile.Wall:
					tileInstance = Instantiate (RandomTile(wallPrefabs), position, Quaternion.identity) as GameObject;
					break;
				case Level.Tile.OuterWall:
					tileInstance = Instantiate (RandomTile (outerWallPrefabs), position, Quaternion.identity) as GameObject;
					break;
				}

				if (tileInstance != null) {
					tileInstance.transform.parent = boardHolder.transform;
				}
			}
		}
	}

	private GameObject RandomTile(GameObject[] tiles) {
		return tiles[UnityEngine.Random.Range (0, tiles.Length)];
	}
}

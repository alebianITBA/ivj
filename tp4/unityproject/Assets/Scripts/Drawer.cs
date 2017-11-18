using UnityEngine;

public class Drawer : MonoBehaviourSingleton<Drawer> {
	public GameObject[] floorPrefabs;
	public GameObject[] wallPrefabs;
	public GameObject[] outerWallPrefabs;
	public GameObject playerPrefab;

	[HideInInspector]
	public float tileLength;
	private float halfTileLength;

	override protected void Initialize() {
		tileLength = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x - 0.01f;
		halfTileLength = tileLength / 2f;
	}

	public void DrawTiles(Level level, GameObject holder) {
		for (int row = 0; row < level.GetMap().GetLength(0); row++) {
			for (int col = 0; col < level.GetMap().GetLength(1); col++) {
				GameObject tileInstance = null;
				Vector3 position = new Vector3 (row * tileLength, col * tileLength, 10);

				switch (level.GetMap() [row, col]) {
				case Level.Tile.Floor:
					tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), position);
					break;
				case Level.Tile.PlayerSpawn:
					// TODO: Make a different floor sprite
					tileInstance = NewObjectFromPrefab (RandomTile(floorPrefabs), position);
					break;
				case Level.Tile.ZombieSpawn:
					// TODO: Make a different floor sprite
					tileInstance = NewObjectFromPrefab (RandomTile(floorPrefabs), position);
					break;
				case Level.Tile.Wall:
					tileInstance = NewObjectFromPrefab (RandomTile (wallPrefabs), position);
					break;
				case Level.Tile.OuterWall:
					tileInstance = NewObjectFromPrefab (RandomTile (outerWallPrefabs), position);
					break;
				}

				if (tileInstance != null) {
					tileInstance.transform.parent = holder.transform;
				}
			}
		}
	}

	public GameObject CreatePlayer(LevelPosition position) {
		return NewObjectFromPrefab (playerPrefab, NewPlayerPosition(position));
	}

	// Given a player and the direction where it should appear, it resets it's position
	public void RepositionPlayer(GameObject player, Level.Direction direction, Level level) {
		LevelPosition np = null;

		switch (direction) {
			case Level.Direction.South:
				np = new LevelPosition (Mathf.FloorToInt(player.transform.position.x * 100 % tileLength * 100), 0);
				break;
			case Level.Direction.North:
				np = new LevelPosition (Mathf.FloorToInt(player.transform.position.x * 100 % tileLength * 100), level.GetMap().GetLength(1) - 1);
				break;
			case Level.Direction.East:
				np = new LevelPosition (level.GetMap().GetLength(0) - 1, Mathf.FloorToInt(player.transform.position.y * 100 % tileLength * 100));
				break;
			case Level.Direction.West:
				np = new LevelPosition (0, Mathf.FloorToInt(player.transform.position.y * 100  % tileLength * 100));
				break;
		}

		if (level.GetMap () [np.x, np.y] == Level.Tile.Wall) {
			int xd = 0;
			int yd = 0;
			bool looking = true;

			switch (direction) {
				case Level.Direction.South:
					while (looking) {
						if ((np.x + xd) > level.GetMap ().GetLength (0) && (np.x - xd) < 0) {
							xd = 0;
							yd++;
						} else {
							if ((np.x + xd) < level.GetMap().GetLength(0) && level.GetMap () [np.x + xd, np.y + yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x + xd, np.y + yd);
								break;
							}
							if ((np.x - xd) >= 0 && level.GetMap () [np.x - xd, np.y + yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x - xd, np.y + yd);
								break;
							}
							xd++;
						}
					}
					break;
				case Level.Direction.North:
					while (looking) {
						if ((np.x + xd) > level.GetMap ().GetLength (0) && (np.x - xd) < 0) {
							xd = 0;
							yd++;
						} else {
							if ((np.x + xd) < level.GetMap().GetLength(0) && level.GetMap () [np.x + xd, np.y - yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x + xd, np.y - yd);
								break;
							}
							if ((np.x - xd) >= 0 && level.GetMap () [np.x - xd, np.y - yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x - xd, np.y - yd);
								break;
							}
							xd++;
						}
					}
					break;
				case Level.Direction.East:
					while (looking) {
						if ((np.y + yd) > level.GetMap ().GetLength (1) && (np.y - yd) < 0) {
							yd = 0;
							xd++;
						} else {
							if ((np.y + yd) < level.GetMap().GetLength(1) && level.GetMap () [np.x - xd, np.y + yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x - xd, np.y + yd);
								break;
							}
							if ((np.y - yd) >= 0 && level.GetMap () [np.x - xd, np.y - yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x - xd, np.y - yd);
								break;
							}
							xd++;
						}
					}
					break;
				case Level.Direction.West:
					while (looking) {
						if ((np.y + yd) > level.GetMap ().GetLength (1) && (np.y - yd) < 0) {
							yd = 0;
							xd++;
						} else {
							if ((np.y + yd) < level.GetMap().GetLength(1) && level.GetMap () [np.x + xd, np.y + yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x + xd, np.y + yd);
								break;
							}
							if ((np.y - yd) >= 0 && level.GetMap () [np.x + xd, np.y - yd] != Level.Tile.Wall) {
								np = new LevelPosition (np.x + xd, np.y - yd);
								break;
							}
							xd++;
						}
					}
					break;
			}	
		}

		player.transform.position = NewPlayerPosition (np);
	}

	private Vector3 NewPlayerPosition(LevelPosition position) {
		return new Vector3 ((position.x * tileLength) + halfTileLength, (position.y * tileLength) + halfTileLength, 0);
	}

	public float MaxVisibleX(Level level) {
		return level.GetMap ().GetLength (0) * tileLength;
	}

	public float MaxVisibleY(Level level) {
		return level.GetMap ().GetLength (1) * tileLength;
	}

	public LevelPosition GetLevelPosition(GameObject obj) {
		int x = Mathf.FloorToInt(obj.transform.position.x % tileLength);
		int y = Mathf.FloorToInt(obj.transform.position.x % tileLength);
		return new LevelPosition (x, y);
	}

	private GameObject NewObjectFromPrefab(GameObject prefab, Vector3 position) {
		return Instantiate (prefab, position, Quaternion.identity) as GameObject;
	}

	private GameObject RandomTile(GameObject[] tiles) {
		return tiles[UnityEngine.Random.Range (0, tiles.Length)];
	}
}

using UnityEngine;

public class Drawer : MonoBehaviourSingleton<Drawer> {
	// COLORS
	public static Color YELLOW = new Color(1f, 1f, 0f);
	public static Color RED = new Color(1f, 0f, 0f);
	public static Color DARK_RED = new Color(0.4f, 0f, 0f);
	public static Color GREEN = new Color(0f, 1f, 0f);
	public static Color DARK_GREEN = new Color(0f, 0.4f, 0f);
	public static Color BLUE = new Color(0f, 0f, 1f);
	public static Color DARK_BLUE = new Color(0f, 0f, 0.4f);
	public static Color BLACK = new Color(0f, 0f, 0f);
	public static Color WHITE = new Color(1f, 1f, 1f);
	public static Color PURPLE = new Color(1f, 0f, 1f);

	private static Color MM_BORDER_COLOR = YELLOW;
	private static Color MM_WALL_COLOR = WHITE;
	private static Color MM_OUTER_WALL_COLOR = WHITE;
	private static Color MM_FLOOR_COLOR = BLACK;

	private static Color MM_PLAYER_COLOR = GREEN;
	private static Color MM_ZOMBIE_COLOR = RED;

	private static Color MM_ZOMBIE_SPAWN_COLOR = DARK_RED;
	private static Color MM_AMMO_SPAWN_COLOR = DARK_BLUE;
	private static Color MM_HEALTH_SPAWN_COLOR = DARK_GREEN;
	private static Color MM_SPECIAL_BOX_SPAWN_COLOR = PURPLE;

	public GameObject[] floorPrefabs;
	public GameObject[] wallPrefabs;
	public GameObject[] outerWallPrefabs;
	public GameObject playerPrefab;
	public GameObject minimap;
	public GameObject ammoPrefab;
	public GameObject healthKitPrefab;
	public GameObject specialBoxPrefab;

	[HideInInspector]
	public float tileLength;
	private float halfTileLength;

	override protected void Initialize() {
		tileLength = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x - 0.01f;
		halfTileLength = tileLength / 2f;
	}

	public void DrawTiles(Level level, GameObject tilesHolder, GameObject accessoriesHolder) {
		for (int row = 0; row < level.GetMap().GetLength(0); row++) {
			for (int col = 0; col < level.GetMap().GetLength(1); col++) {
				GameObject tileInstance = null;
				Vector3 position = new Vector3 (row * tileLength, col * tileLength, 10);
//				Vector3 midPosition = new Vector3 ((row * tileLength) + halfTileLength, (col * tileLength) + halfTileLength, 10);

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
					case Level.Tile.AmmoSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), position);
						GameObject ammo = NewObjectFromPrefab (ammoPrefab, position);
						ammo.transform.parent = accessoriesHolder.transform;
						break;
					case Level.Tile.HealthKitSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), position);
						GameObject healthKit = NewObjectFromPrefab (healthKitPrefab, position);
						healthKit.transform.parent = accessoriesHolder.transform;
						break;
					case Level.Tile.SpecialBoxSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), position);
						GameObject box = NewObjectFromPrefab (specialBoxPrefab, position);
						box.transform.parent = accessoriesHolder.transform;
						break;
				}

				if (tileInstance != null) {
					tileInstance.transform.parent = tilesHolder.transform;
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
		int tl = Mathf.FloorToInt (tileLength * 100);
		int x = Mathf.FloorToInt (obj.transform.position.x * 100);
		int y = Mathf.FloorToInt (obj.transform.position.y * 100);

		return new LevelPosition (Mathf.FloorToInt (x / tl), Mathf.FloorToInt (y / tl));
	}

	private GameObject NewObjectFromPrefab(GameObject prefab, Vector3 position) {
		return Instantiate (prefab, position, Quaternion.identity) as GameObject;
	}

	private GameObject RandomTile(GameObject[] tiles) {
		return tiles[UnityEngine.Random.Range (0, tiles.Length)];
	}

	public void DrawMinimap(Level level, GameObject player) {
		Level.Tile[,] map = level.GetMap ();

		Texture2D texture = new Texture2D(map.GetLength(0) + 2, map.GetLength(1) + 2, TextureFormat.RGBA32, false);
		for (int y = 0; y < map.GetLength(1); y++) {
			for (int x = 0; x < map.GetLength(0); x++) {
				switch (map [x, y]) {
					case Level.Tile.Wall:
						texture.SetPixel (x + 1, y + 1, MM_WALL_COLOR);
						break;
					case Level.Tile.ZombieSpawn:
						texture.SetPixel(x + 1, y + 1, MM_ZOMBIE_SPAWN_COLOR);
						break;
					case Level.Tile.AmmoSpawn:
						texture.SetPixel(x + 1, y + 1, MM_AMMO_SPAWN_COLOR);
						break;
					case Level.Tile.HealthKitSpawn:
						texture.SetPixel(x + 1, y + 1, MM_HEALTH_SPAWN_COLOR);
						break;
					case Level.Tile.SpecialBoxSpawn:
						texture.SetPixel(x + 1, y + 1, MM_SPECIAL_BOX_SPAWN_COLOR);
						break;
					default:
						texture.SetPixel(x + 1, y + 1, MM_FLOOR_COLOR);
						break;
				}
			}
		}
		// Paint borders
		for (int x = 0; x < map.GetLength(0) + 2; x++) {
			texture.SetPixel(x, 0, MM_BORDER_COLOR);
			texture.SetPixel(x, map.GetLength(1) + 1, MM_BORDER_COLOR);
		}
		for (int y = 0; y < map.GetLength(1) + 2; y++) {
			texture.SetPixel(0, y, MM_BORDER_COLOR);
			texture.SetPixel(map.GetLength(0) + 1, y, MM_BORDER_COLOR);
		}
		// Paint player
		LevelPosition playerPosition = GetLevelPosition(player);
		texture.SetPixel(playerPosition.x + 1, playerPosition.y + 1, MM_PLAYER_COLOR);

		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();

		minimap.GetComponent<GUITexture>().texture = texture;
	}
}

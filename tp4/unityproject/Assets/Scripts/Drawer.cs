using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

	// Prefabs
	public GameObject[] floorPrefabs;
	public GameObject[] wallPrefabs;
	public GameObject[] outerWallPrefabs;
	public GameObject playerPrefab;
	public GameObject minimap;
	public GameObject ammoPrefab;
	public GameObject healthKitPrefab;
	public GameObject specialBoxPrefab;
	public GameObject actionText;

	private GameObject player;

	[HideInInspector]
	public float tileLength;
	private float halfTileLength;

	public Text scoreText;
	public Text lifeText;
	public Text bulletsText;

	private List<TimeDestroyable> destroyables;

	override protected void Initialize() {
		this.destroyables = new List<TimeDestroyable> ();
		tileLength = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x - 0.01f;
		halfTileLength = tileLength / 2f;
	}

	void Update() {
		if (player != null) {
			scoreText.text = "Score: " + player.GetComponent<Character>().score.ToString();
			lifeText.text = "Life: " + Mathf.Max(GameLogic.PLAYERHEALTH, player.GetComponent<Character>().health).ToString();
			bulletsText.text = "Bullets: " + player.GetComponent<Character>().bullets.ToString();
		}
		for (int i = destroyables.Count - 1; i >= 0; i--) {
			if (destroyables [i].Destroyable ()) {
				Destroy (destroyables [i].obj);
				destroyables.RemoveAt (i);
			}
		}
	}

	public void DrawTiles(Level level, GameObject tilesHolder, GameObject accessoriesHolder) {
		for (int row = 0; row < level.GetMap().GetLength(0); row++) {
			for (int col = 0; col < level.GetMap().GetLength(1); col++) {
				GameObject tileInstance = null;
				Vector3 position = new Vector3 (row * tileLength, col * tileLength, 10);

				switch (level.GetMap() [row, col]) {
					case Level.Tile.Floor:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.PlayerSpawn:
						// TODO: Make a different floor sprite
						tileInstance = NewObjectFromPrefab (RandomTile(floorPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.ZombieSpawn:
						// TODO: Make a different floor sprite
						tileInstance = NewObjectFromPrefab (RandomTile(floorPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.Wall:
						tileInstance = NewObjectFromPrefab (RandomTile (wallPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.OuterWall:
						tileInstance = NewObjectFromPrefab (RandomTile (outerWallPrefabs), tilesHolder.transform);
						break;
					case Level.Tile.AmmoSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						GameObject ammo = NewObjectFromPrefab (ammoPrefab, tilesHolder.transform);
						ammo.transform.localPosition = position;
						BulletManager.Instance.IgnoreColliders(ammo.GetComponent<Collider2D>());
						break;
					case Level.Tile.HealthKitSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						GameObject healthKit = NewObjectFromPrefab (healthKitPrefab, tilesHolder.transform);
						healthKit.transform.localPosition = position;
						BulletManager.Instance.IgnoreColliders(healthKit.GetComponent<Collider2D>());
						break;
					case Level.Tile.SpecialBoxSpawn:
						tileInstance = NewObjectFromPrefab (RandomTile (floorPrefabs), tilesHolder.transform);
						GameObject box = NewObjectFromPrefab (specialBoxPrefab, tilesHolder.transform);
						box.transform.localPosition = position;
						BulletManager.Instance.IgnoreColliders(box.GetComponent<Collider2D>());
						break;
				}

				if (tileInstance != null) {
					//tileInstance.transform.parent = holder.transform;
					tileInstance.transform.localPosition = position;
				}
			}
		}
	}

	public GameObject CreatePlayer(LevelPosition position) {
		player = NewObjectFromPrefab (playerPrefab, NewPlayerPosition(position));
		return player;
	}

	public void RepositionPlayer(GameObject player, Level.Direction direction, Level level, GameObject holder) {
		player.transform.parent = holder.transform;
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

	private GameObject NewObjectFromPrefab(GameObject prefab, Transform parent) {
		return Instantiate (prefab, parent) as GameObject;
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

	public void CreateActionText(string text, Color color, Vector3 position) {
		position.z = -10;
		GameObject newText = NewObjectFromPrefab (actionText, position);
		newText.GetComponent<TextMesh> ().text = text;
		newText.GetComponent<TextMesh> ().color = color;
		destroyables.Add (new TimeDestroyable(newText, 500));
	}
}

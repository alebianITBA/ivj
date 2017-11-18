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

	public void RepositionPlayer(GameObject player, LevelPosition position) {
		player.transform.position = NewPlayerPosition(position);
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

	private GameObject NewObjectFromPrefab(GameObject prefab, Vector3 position) {
		return Instantiate (prefab, position, Quaternion.identity) as GameObject;
	}

	private GameObject RandomTile(GameObject[] tiles) {
		return tiles[UnityEngine.Random.Range (0, tiles.Length)];
	}
}

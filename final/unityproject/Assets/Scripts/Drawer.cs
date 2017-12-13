using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviourSingleton<Drawer> {
	public GameObject[] floorPrefabs;
	public GameObject[] wallPrefabs;
	public GameObject tilesHolder;

	[HideInInspector]
	public float tileLength;
	private float halfTileLength;

	override protected void Initialize() {
		tileLength = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x - 0.01f;
		halfTileLength = tileLength / 2f;
	}

	public void DrawMap(Level.Tile[,] map) {
		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				GameObject tileInstance = null;
				Vector3 position = new Vector3 (row * tileLength, col * tileLength, 10);

				switch (map [row, col]) {
				case Level.Tile.Wall:
					tileInstance = NewObjectFromPrefab (RandomTile (wallPrefabs), tilesHolder.transform);
					break;
				default:
					tileInstance = NewObjectFromPrefab (RandomTile(floorPrefabs), tilesHolder.transform);
					break;
				}

				if (tileInstance != null) {
					//tileInstance.transform.parent = holder.transform;
					tileInstance.transform.localPosition = position;
				}
			}
		}
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviourSingleton<Drawer>
{
    public GameObject[] floorPrefabs;
    public GameObject[] wallPrefabs;
    public GameObject basePrefab;
    public GameObject towerPrefab;
    public GameObject minionSpawnPrefab;
    public GameObject tilesHolder;

    [HideInInspector]
    public float tileLength;
    private float halfTileLength;

    private Level level;
    private List<LevelPosition> baseTiles;
    private List<LevelPosition> towerTiles;
    private List<LevelPosition> minionTiles;

    override protected void Initialize ()
    {
        tileLength = floorPrefabs[0].GetComponent<Renderer>().bounds.size.x - 0.01f;
        halfTileLength = tileLength / 2f;
        this.baseTiles = new List<LevelPosition>();
        this.towerTiles = new List<LevelPosition>();
        this.minionTiles = new List<LevelPosition>();
    }

    public void SetLevel (Level level)
    {
        this.level = level;
    }

    public void DrawMap ()
    {
        for (int row = 0; row < this.level.GetMap().GetLength(0); row++) {
            for (int col = 0; col < this.level.GetMap().GetLength(1); col++) {
                GameObject tileInstance = null;
                Vector3 position = new Vector3(row * tileLength, col * tileLength, 10);

                switch (this.level.GetMap()[row, col]) {
                    case Level.Tile.Wall:
                        tileInstance = NewObjectFromPrefab(RandomTile(wallPrefabs), tilesHolder.transform);
                        break;
                    case Level.Tile.BaseTower:
                        baseTiles.Add(new LevelPosition(row, col));
                        tileInstance = NewObjectFromPrefab(RandomTile(floorPrefabs), tilesHolder.transform);
                        break;
                    case Level.Tile.Tower:
                        towerTiles.Add(new LevelPosition(row, col));
                        tileInstance = NewObjectFromPrefab(RandomTile(floorPrefabs), tilesHolder.transform);
                        break;
                    case Level.Tile.MinionSpawn:
                        minionTiles.Add(new LevelPosition(row, col));
                        tileInstance = NewObjectFromPrefab(RandomTile(floorPrefabs), tilesHolder.transform);
                        break;
                    default:
                        tileInstance = NewObjectFromPrefab(RandomTile(floorPrefabs), tilesHolder.transform);
                        break;
                }

                if (tileInstance != null) {
                    //tileInstance.transform.parent = holder.transform;
                    tileInstance.transform.localPosition = position;
                }
            }
        }
    }

    public void DrawObjectives ()
    {
        foreach (LevelPosition tile in towerTiles) {
            GameObject tower = NewObjectFromPrefab(towerPrefab, GetExactPosition(tile));
        }
        foreach (LevelPosition tile in baseTiles) {
            GameObject tower = NewObjectFromPrefab(basePrefab, GetExactPosition(tile));
        }
        foreach (LevelPosition tile in minionTiles) {
            GameObject tower = NewObjectFromPrefab(minionSpawnPrefab, GetExactPosition(tile));
        }
    }

    public Vector3 GetPositionAndHalf (LevelPosition position)
    {
        return new Vector3((position.x * tileLength) + halfTileLength, (position.y * tileLength) + halfTileLength, 0);
    }

    public Vector3 GetExactPosition (LevelPosition position)
    {
        return new Vector3((position.x * tileLength), (position.y * tileLength), 0);
    }

    private GameObject NewObjectFromPrefab (GameObject prefab, Vector3 position)
    {
        return Instantiate(prefab, position, Quaternion.identity) as GameObject;
    }

    private GameObject NewObjectFromPrefab (GameObject prefab, Transform parent)
    {
        return Instantiate(prefab, parent) as GameObject;
    }

    private GameObject RandomTile (GameObject[] tiles)
    {
        return tiles[UnityEngine.Random.Range(0, tiles.Length)];
    }
}

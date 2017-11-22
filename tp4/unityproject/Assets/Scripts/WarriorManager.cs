using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WarriorManager : MonoBehaviourSingleton<WarriorManager> {

    public GameObject warriorPrefab;
    System.DateTime lastSpawn;
    int spawned = 0;
	List<Level.Direction> directions;

    private Queue<Warrior> warriorPool;

    // Use this for initialization
    void Start () {
        PrecreateObjects();
		directions = new List<Level.Direction> () {Level.Direction.North,
			Level.Direction.East, Level.Direction.West, Level.Direction.South, Level.Direction.NorthEast,
			Level.Direction.NorthWest, Level.Direction.SouthEast, Level.Direction.SouthWest};
    }
	
    private void PrecreateObjects()
    {
        warriorPool = new Queue<Warrior>();
		for (int i = 0; i < GameLogic.WARRIOR_AMOUNT; i++)
        {
			GameObject go = GameObject.Instantiate(warriorPrefab) as GameObject;
			Warrior warrior = go.GetComponent<Warrior>();
            warrior.SetManager(this);
            if (warrior == null)
            {
                Debug.LogError("Cannot fint the component Zombie in the zombie prefab.");
            }
            go.name = warriorPrefab.name;
            go.transform.parent = transform;
            go.SetActive(false);
            warrior.container = go;
            warrior.inQueue = true;
			warriorPool.Enqueue(warrior);
        }
    }
    void Update() {
		if (!GameLogic.Instance.IsPaused ()) {
			System.TimeSpan ts = System.DateTime.Now - lastSpawn;
			if (ts.TotalMilliseconds > GameLogic.Instance.ZombieTimeSpawn () && warriorPool.Count > 0) {
				int i = UnityEngine.Random.Range (0, directions.Count);
				Level.Direction dir = directions [i];
				Vector2 spawnPos = getSpawnPosition (CrazyCaveLevelManager.Instance.GetLevel (dir));
				if (Math.Abs (Vector2.Distance (spawnPos, CrazyCaveGameManager.Instance.player.transform.position)) > GameLogic.WARRIOR_SPAWN_DISTANCE) {
					spawned++;
					lastSpawn = System.DateTime.Now;
					Warrior warrior = warriorPool.Dequeue ();
					warrior.transform.SetParent (CrazyCaveLevelManager.Instance.GetHolder (dir).transform);
					warrior.transform.localPosition = spawnPos;
					warrior.SetLevel (CrazyCaveLevelManager.Instance.GetLevel (dir));
					CrazyCaveLevelManager.Instance.GetLevel (dir).AddWarrior (warrior);
					warrior.gameObject.SetActive (true);
					if (UnityEngine.Random.value < 0.05) {
						SoundManager.PlayBackground ((int)SndIdGame.ZOMBIE_SPAWN);
					}
				}
			}
		}
	}

	public void RecycleWarrior(Warrior warrior)
    {
		warrior.transform.SetParent (transform);
		warriorPool.Enqueue(warrior);
        warrior.inQueue = true;
		warrior.gameObject.SetActive(false);
		warrior.SetAlive();
		warrior.GetComponent<Collider2D> ().enabled = true;
		warrior.GetLevel ().RemoveWarrior (warrior);
    }

	private static Vector2 getSpawnPosition(Level level) {
		List<LevelPosition> possibleSpawns= level.GetAvailableZombieSpawnPositions ();
		int i = UnityEngine.Random.Range (0, possibleSpawns.Count);
		return Drawer.Instance.GetPosition (possibleSpawns [i]);
	}

    public void IgnoreColliders(Collider2D collider) {
        foreach(Warrior w in warriorPool) {
            Physics2D.IgnoreCollision (collider, w.GetComponent<Collider2D>());
        }
    }

    public List<Warrior> GetWarriors() {
        return new List<Warrior>(warriorPool.ToArray());
    }
}

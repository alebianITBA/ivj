using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorManager : MonoBehaviourSingleton<WarriorManager> {

    public GameObject warriorPrefab;
    System.DateTime lastSpawn;
    int spawned = 0;

    private Queue<Warrior> warriorPool;

    // Use this for initialization
    void Start () {
        PrecreateObjects();
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
        System.TimeSpan ts = System.DateTime.Now - lastSpawn;
		if (ts.TotalMilliseconds > GameLogic.Instance.ZombieTimeSpawn() && warriorPool.Count > 0)
        {
            spawned++;
            lastSpawn = System.DateTime.Now;
			Warrior warrior = warriorPool.Dequeue();
            warrior.inQueue = false;
			warrior.transform.position = (Vector2)CrazyCaveGameManager.Instance.player.transform.position + getSpawnPosition(GameLogic.WARRIOR_SPAWN_DISTANCE);
			warrior.gameObject.SetActive(true);
        }
    }

	public void RecycleWarrior(Warrior warrior)
    {
		warriorPool.Enqueue(warrior);
        warrior.inQueue = true;
		warrior.gameObject.SetActive(false);
		warrior.SetAlive();
		warrior.GetComponent<Collider2D> ().enabled = true;
    }

	private static Vector2 getSpawnPosition(float distance) {
		float x = Random.Range (-1.0f, 1.0f);
		float y = Random.Range (-1.0f, 1.0f);

		Vector2 dir = new Vector2 (x, y);
		return dir.normalized * distance;
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

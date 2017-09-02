using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {

    public GameObject zombiePrefab;
    System.DateTime lastSpawn;
    int spawned = 0;

    private Queue<ZombieController> zombiePool;

    // Use this for initialization
    void Start () {
        PrecreateObjects();
    }
	
    private void PrecreateObjects()
    {
        zombiePool = new Queue<ZombieController>();
		for (int i = 0; i < GameLogic.ZOMBIE_AMOUNT; i++)
        {
            GameObject go = GameObject.Instantiate(zombiePrefab) as GameObject;
            ZombieController zombie = go.GetComponent<ZombieController>();
            zombie.SetManager(this);
            if (zombie == null)
            {
                Debug.LogError("Cannot fint the component Zombie in the zombie prefab.");
            }
            go.name = zombiePrefab.name;
            go.transform.parent = transform;
            go.SetActive(false);
            zombiePool.Enqueue(zombie);
        }
    }
    void Update() {
        System.TimeSpan ts = System.DateTime.Now - lastSpawn;
		if (ts.TotalMilliseconds > GameLogic.ZOMBIE_TIME_BETWEEN_SPAWNS && zombiePool.Count > 0)
        {
            spawned++;
            lastSpawn = System.DateTime.Now;
            ZombieController zombie = zombiePool.Dequeue();
            zombie.transform.position = new Vector2(0.0f, 0.0f);
			zombie.gameObject.SetActive(true);
        }
    }

    public void RecycleZombie(ZombieController zombie)
    {
        zombiePool.Enqueue(zombie);
        zombie.gameObject.SetActive(false);
		zombie.SetAlive();
		zombie.GetComponent<PolygonCollider2D> ().enabled = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {

    public GameObject zombiePrefab;
    private int ZOMBIE_LIMIT = 100;

    System.DateTime lastSpawn;
    int spawned = 0;

    private Queue<ZombieController> zombiePool;

    private double TIME_BETWEEN_SPAWNS = 5000.0f;

    // Use this for initialization
    void Start () {
        PrecreateObjects();
    }
	
    private void PrecreateObjects()
    {
        print("creating zombie pool");
        zombiePool = new Queue<ZombieController>();
        for (int i = 0; i < ZOMBIE_LIMIT; i++)
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
        if (ts.TotalMilliseconds > TIME_BETWEEN_SPAWNS && zombiePool.Count > 0)
        {
            spawned++;
            lastSpawn = System.DateTime.Now;
            ZombieController bul = zombiePool.Dequeue();
            bul.transform.position = new Vector2(0.0f, 0.0f);
            bul.gameObject.SetActive(true);
        }
    }

    public void RecycleZombie(ZombieController zombie)
    {
        print("RECYCLE");
        zombiePool.Enqueue(zombie);
        zombie.gameObject.SetActive(false);
    }
}

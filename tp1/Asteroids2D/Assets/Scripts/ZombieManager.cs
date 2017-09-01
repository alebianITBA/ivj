using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {

    public GameObject zombiePrefab;
    private int ZOMBIE_LIMIT = 10;

    System.DateTime lastSpawn;
    int spawned = 0;

    private Queue<ZombieController> zombiePool;

    private double TIME_BETWEEN_SHOTS = 10000.0f;

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
            ZombieController bul = go.GetComponent<ZombieController>();
            if (bul == null)
            {
                Debug.LogError("Cannot fint the component Zombie in the zombie prefab.");
            }
            go.name = zombiePrefab.name;
            go.transform.parent = transform;
            go.SetActive(false);
            zombiePool.Enqueue(bul);
        }
    }
    void Update() {
        System.TimeSpan ts = System.DateTime.Now - lastSpawn;
        if (ts.TotalMilliseconds > TIME_BETWEEN_SHOTS && zombiePool.Count > 0)
        {
            spawned++;
            lastSpawn = System.DateTime.Now;
            ZombieController bul = zombiePool.Dequeue();
            bul.transform.position = new Vector2(0.0f, 0.0f);
            bul.gameObject.SetActive(true);
        }
    }

    public void RecycleBullet(ZombieController bul)
    {
        zombiePool.Enqueue(bul);
        bul.gameObject.SetActive(false);
    }
}

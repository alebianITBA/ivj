using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager
{
    private GameObject rocketPrefab;
    private GameManager.Teams team;
    private Queue<Rocket> rocketPool;
    private List<GameObject> rocketObjects;

    public RocketManager (int amount, GameObject rocketPrefab, GameManager.Teams team)
    {
        this.rocketPrefab = rocketPrefab;
        this.team = team;
        PrecreateObjects(amount);
    }

    private void PrecreateObjects (int amount)
    {
        rocketPool = new Queue<Rocket>();
        rocketObjects = new List<GameObject>();
        for (int i = 0; i < amount; i++) {
            GameObject go = GameObject.Instantiate(rocketPrefab) as GameObject;
            Rocket bul = go.GetComponent<Rocket>();
            bul.SetManager(this);
            bul.SetTeam(team);
            if (bul == null) {
                Debug.LogError("Cannot fint the component Rocket in the rocket prefab.");
            }
            go.name = "Rocket";
            go.SetActive(false);
            rocketPool.Enqueue(bul);
            rocketObjects.Add(go);
            IgnoreColliders(go.GetComponent<BoxCollider2D>());
        }
    }

    public void Shoot (Vector2 pos, Vector3 rot, Vector2 dir, Quaternion rotation)
    {
        Rocket bul = rocketPool.Dequeue();
        bul.ShootedAt = System.DateTime.Now;
        bul.transform.rotation = rotation;
        bul.transform.position = pos;
        bul.transform.Rotate(0, 0, rot.z - 90);
        bul.gameObject.SetActive(true);
        bul.GetComponent<Rigidbody2D>().AddForce(dir * Constants.BULLET_SPEED);
    }

    public void RecycleRocket (Rocket bul)
    {
        rocketPool.Enqueue(bul);
        bul.gameObject.SetActive(false);
    }

    public void IgnoreColliders (Collider2D collider)
    {
        if (rocketPool == null) {
            return;
        }

        foreach (GameObject b in rocketObjects) {
            Physics2D.IgnoreCollision(collider, b.GetComponent<BoxCollider2D>());
        }
    }

    public int RocketsLeft ()
    {
        return rocketPool.Count;
    }
}


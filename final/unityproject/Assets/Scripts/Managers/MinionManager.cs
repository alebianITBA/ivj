using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MinionManager : MonoBehaviour
{

    public GameObject minionPrefab;
    System.DateTime lastSpawn;
    int spawned = 0;
    private GameManager.Teams team;

    private Queue<Minion> minionPool;
    // Use this for initialization
    void Start ()
    {
        PrecreateObjects();
    }

    public void SetTeam (GameManager.Teams team)
    {
        this.team = team;
    }

    private void PrecreateObjects ()
    {
        minionPool = new Queue<Minion>();
        for (int i = 0; i < Constants.MINION_AMOUNT; i++) {
            GameObject go = GameObject.Instantiate(minionPrefab) as GameObject;
            Minion minion = go.GetComponent<Minion>();
            minion.SetTeam(this.team);
            minion.SetManager(this);
            if (minion == null) {
                Debug.LogError("Cannot fint the component Minion in the minion prefab.");
            }
            go.name = minionPrefab.name;
            go.transform.parent = transform;
            go.SetActive(false);
            minionPool.Enqueue(minion);
            if (this.team == GameManager.Teams.RED) {
                GameManager.Instance.REDMinions.Add(go);
            }
            else {
                GameManager.Instance.BLUEMinions.Add(go);
            }
        }
    }

    public void Spawn ()
    {
        System.TimeSpan ts = System.DateTime.Now - lastSpawn;
        if (ts.TotalMilliseconds > Constants.MINION_COOLDOWN && minionPool.Count > 0) {
            spawned++;
            lastSpawn = System.DateTime.Now;
            Minion minion = minionPool.Dequeue();
            minion.Heal(Constants.MINION_MAX_BASE_HEALTH);
            minion.transform.position = transform.position;
            minion.gameObject.SetActive(true);
            //SoundManager.PlayBackground ((int)SndIdGame.MINION_SPAWN);
        }
    }

    public void RecycleMinion (Minion minion)
    {
        minion.transform.SetParent(transform);
        minionPool.Enqueue(minion);
        //minion.inQueue = true;
        minion.gameObject.SetActive(false);
        //minion.SetAlive();
        minion.GetComponent<Collider2D>().enabled = true;
    }


    public void IgnoreColliders (Collider2D collider)
    {
        foreach (Minion w in minionPool) {
            Physics2D.IgnoreCollision(collider, w.GetComponent<Collider2D>());
        }
    }

    public List<Minion> GetMinions ()
    {
        return new List<Minion>(minionPool.ToArray());
    }
  
}

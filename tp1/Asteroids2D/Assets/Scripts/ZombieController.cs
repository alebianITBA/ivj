using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {
    private GameObject player;
    Rigidbody2D rb;
    float VELOCITY = 10f;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	void Update () {
        Vector2 playerPosition = GameObject.Find("Character").transform.position;
        Vector2 myPosition = transform.position;
        Vector2 direction = playerPosition - myPosition;
        print(direction.normalized * VELOCITY);
        rb.AddForce(direction.normalized * VELOCITY);
    }
}

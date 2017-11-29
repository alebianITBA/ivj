using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningFire : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.name == "Player(Clone)") {
			col.gameObject.GetComponentInChildren<Character>().TakeDamage ();
		}
	}

	void OnTriggerStay2D(Collider2D col) {
		if (col.gameObject.name == "Player(Clone)") {
			col.gameObject.GetComponentInChildren<Character>().TakeDamage ();
			}
	}
}

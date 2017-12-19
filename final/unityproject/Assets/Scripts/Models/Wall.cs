using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.gameObject.name == "Bullet") {
            col.gameObject.GetComponent<Bullet>().Recycle();
        }
    }

	void OnTriggerEnter2D (Collider2D col){
		if (col.gameObject.name == "Rocket") {
			col.gameObject.GetComponent<Rocket>().Recycle();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject scoreController;
	// Use this for initialization
	void Awake () {
		if (HighscoreController.instance == null) {
			Instantiate (scoreController);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

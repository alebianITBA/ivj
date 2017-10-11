using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviourSingleton<GameController> {
	public CameraManager cameraManager;
	public CueManager cueManager;

	private static float ENERGY = 100.0f;
	private static float MAX_ENERGY = 30000.0f;

	public float currentPlayerEnergy = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (StateManager.Instance.Paused ()) {

		} else {
			if (Input.GetKey (KeyCode.LeftArrow)) {
				cameraManager.RotateLeft ();
				cueManager.RotateLeft ();
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				cameraManager.RotateRight ();
				cueManager.RotateRight ();
			}

			// Shot
		}
	}


}

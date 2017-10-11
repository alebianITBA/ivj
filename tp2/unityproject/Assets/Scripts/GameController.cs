using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController> {
	public CameraManager cameraManager;
	public CueManager cueManager;
	public GameObject energyBar;

	private static float ENERGY = 100.0f;
	private static float MAX_ENERGY = 30000.0f;

	public float currentPlayerEnergy = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		energyBar.GetComponent<GUIBarScript>().SetNewValue(currentPlayerEnergy / MAX_ENERGY);

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

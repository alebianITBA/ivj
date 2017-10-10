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
			if (Input.GetKeyDown (KeyCode.Escape)) {
				StateManager.Instance.ContinueGame ();
			}
		} else {
			if (Input.GetKey (KeyCode.LeftArrow)) {
				cameraManager.RotateLeft ();
				cueManager.RotateLeft ();
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				cameraManager.RotateRight ();
				cueManager.RotateRight ();
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				StateManager.Instance.PauseGame ();
			}
			// Shot
			if (StateManager.Instance.Striking ()) {
				if (Input.GetKey (KeyCode.Space)) {
					energyBar.SetActive (true);
					cueManager.speed = 1.0f;
					currentPlayerEnergy += ENERGY;
					if (currentPlayerEnergy > MAX_ENERGY) {
						currentPlayerEnergy = 0.0f;
					}
				}
				if (Input.GetKeyUp (KeyCode.Space)) {
					energyBar.SetActive (false);
					cueManager.speed = 0.0f;
					cameraManager.whiteBall.GetComponent<Rigidbody> ().AddForce (direction () * currentPlayerEnergy);
					StateManager.Instance.ReduceMovements ();
					currentPlayerEnergy = 0.0f;
					//StateManager.Instance.strike ();
				}
			}
		}
	}

	private Vector3 direction()
	{
		Vector3 forward = cameraManager.camera.transform.forward;
		Vector3 direction = new Vector3 (forward.x, 0.0f, forward.z);
		return direction;
	}
}

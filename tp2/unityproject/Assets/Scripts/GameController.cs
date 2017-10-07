using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController> {
	public CameraManager cameraManager;
	public CueManager cueManager;

	private static float ENERGY = 100.0f;
	private static float MAX_ENERGY = 30000.0f;

	private float currentPlayerEnergy = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		print (currentPlayerEnergy);
		if (StateManager.Instance.Paused ()) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				StateManager.Instance.ContinueGame ();
			}
		} else {
			if (BallManager.Instance.Still () && !StateManager.Instance.Striking ()) {
				StateManager.Instance.ReadyToStrike ();
			}
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
					cueManager.speed = 1.0f;
					if (currentPlayerEnergy < MAX_ENERGY) {
						currentPlayerEnergy += ENERGY;
						if (currentPlayerEnergy > MAX_ENERGY) {
							currentPlayerEnergy = MAX_ENERGY;
						}
					}
				}
				if (Input.GetKeyUp (KeyCode.Space)) {
					cueManager.speed = 0.0f;
					StateManager.Instance.Strike ();
					cameraManager.whiteBall.GetComponent<Rigidbody> ().AddForce (direction () * currentPlayerEnergy);
					StateManager.Instance.ReduceMovements ();
					currentPlayerEnergy = 0.0f;
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

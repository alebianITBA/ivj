using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController> {
	public CameraManager cameraManager;

	private static float ENERGY = 1000.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (StateManager.Instance.Paused ()) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				StateManager.Instance.ContinueGame();
			}
		} else {
			if (Input.GetKey (KeyCode.LeftArrow)) {
				cameraManager.RotateLeft ();
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
				cameraManager.RotateRight ();
			}
			if (Input.GetKey (KeyCode.Space)) {
				cameraManager.whiteBall.GetComponent<Rigidbody> ().AddForce (direction () * ENERGY);
			}
			if (Input.GetKeyDown(KeyCode.Escape)) {
				StateManager.Instance.PauseGame();
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

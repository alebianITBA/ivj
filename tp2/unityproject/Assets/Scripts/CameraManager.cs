using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviourSingleton<CameraManager> {
	public GameObject camera;
	public GameObject whiteBall;

	private static float X_ROTATION = 45.0f;
	private static float DISTANCE = 20.0f;

	private static float SENSITIVITY = 90.0f;

	// Use this for initialization
	void Start () {
		camera.transform.Rotate (X_ROTATION, 0, 0);
		whiteBall = BallManager.Instance.whiteBall;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 whitePosition = whiteBall.transform.position;
		camera.transform.position = whitePosition - camera.transform.forward * DISTANCE;
	}

	public void RotateLeft() {
		camera.transform.RotateAround(whiteBall.transform.position, Vector3.up, -1 * Sensitivity());
	}

	public void RotateRight() {
		camera.transform.RotateAround(whiteBall.transform.position, Vector3.up, Sensitivity());
	}

	private float Sensitivity() {
		return Time.deltaTime * SENSITIVITY;
	}
}

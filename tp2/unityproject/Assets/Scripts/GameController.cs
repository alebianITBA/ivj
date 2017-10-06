using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController> {
	public CameraManager cameraManager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow)) {
			cameraManager.RotateLeft();
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			cameraManager.RotateRight();
		}
		if (Input.GetKey(KeyCode.Space)) {
			cameraManager.whiteBall.GetComponent<Rigidbody> ().AddForce (direction() * 100.0f);
		}
	}

	private Vector3 direction()
	{
		float angle = cameraManager.camera.transform.localRotation.eulerAngles.z;
		float x = Mathf.Cos(angle * Mathf.Deg2Rad);
		float y = Mathf.Sin(angle * Mathf.Deg2Rad);
		return new Vector3 (x, 0.0f, y);
	}
}

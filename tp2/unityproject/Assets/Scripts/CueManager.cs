using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueManager : MonoBehaviourSingleton<CueManager> {
	public GameObject Cue;
	public GameObject WhiteBall;
	public GameObject MainCamera;

	private float DISTANCE;
	private static float SENSITIVITY = 90.0f;
	private static float MAX_DISTANCE = 32.0f;
	private float MIN_DISTANCE = 28.0f;

	private float cueDirection = 1;

	public float speed = 0.0f;
	// Use this for initialization
	void Start () {
		WhiteBall = BallManager.Instance.whiteBall;
		DISTANCE = Vector3.Distance(Cue.transform.position, WhiteBall.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance(Cue.transform.position, WhiteBall.transform.position);
		if (distance > MAX_DISTANCE)
			cueDirection = -1;
		else if (distance < MIN_DISTANCE)
			cueDirection = 1;
		Cue.transform.Translate(Vector3.down * speed * 5.0f * cueDirection * Time.deltaTime);
	}

	public void SetVisible(bool b) {
		Cue.GetComponentInChildren<Renderer> ().enabled = b;
	}
	public void ResetCue() {
		Vector3 whitePosition = WhiteBall.transform.position;
		Cue.transform.position = whitePosition - Cue.transform.up * DISTANCE;
	}

	public void RotateLeft() {
		Cue.transform.RotateAround(WhiteBall.transform.position, Vector3.up, -1 * Sensitivity());
	}

	public void RotateRight() {
		Cue.transform.RotateAround(WhiteBall.transform.position, Vector3.up, Sensitivity());
	}

	private float Sensitivity() {
		return Time.deltaTime * SENSITIVITY;
	}

}

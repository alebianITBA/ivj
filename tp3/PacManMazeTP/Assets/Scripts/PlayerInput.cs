// Mono Framework
using System.Collections.Generic;

// Unity Framework
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	Animator anmCtrl;
	AudioSource audioSrc;

	private Vector3 currentDirection;
	private static float VELOCITY = 100.0f;
	private int multiplyer = 1;
	private Vector3 previousDirection = new Vector3(0, 0, 0);

	// Unity Start Method
	void Start()
	{
		anmCtrl = GetComponent<Animator>();
		audioSrc = GetComponent<AudioSource>();
	}
	
	// Unity Update Method
	void Update()
	{
		int currentX;
		int currentY;
		MapManager.Instance.GetRowCol (transform.position, out currentX, out currentY);

		bool couldEat = MapManager.Instance.EatDot (currentX, currentY);
		if (couldEat) {
			SoundManager.Instance.PlayDotSound ();
		}

		if (Input.GetKey(KeyCode.UpArrow)) {
			if (MapManager.Instance.IsWalkable(currentX - 1, currentY)) {
				anmCtrl.SetInteger("moveDir", (int)MoveDir.Up);
				currentDirection = new Vector3 (0, 1, 0);
				multiplyer = -1;
			}
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			if (MapManager.Instance.IsWalkable(currentX + 1, currentY)) {
				anmCtrl.SetInteger("moveDir", (int)MoveDir.Down);
				currentDirection = new Vector3 (0, -1, 0);
				multiplyer = -1;
			}
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			if (MapManager.Instance.IsWalkable(currentX, currentY - 1)) {
				anmCtrl.SetInteger("moveDir", (int)MoveDir.Left);
				currentDirection = new Vector3 (-1, 0, 0);
				multiplyer = 1;
			}
		}
		if (Input.GetKey(KeyCode.RightArrow)) {
			if (MapManager.Instance.IsWalkable(currentX, currentY + 1)) {
				anmCtrl.SetInteger("moveDir", (int)MoveDir.Right);
				currentDirection = new Vector3 (1, 0, 0);
				multiplyer = 1;
			}
		}

		if (previousDirection != currentDirection) {
			transform.position = MapManager.Instance.GetWorldPos (currentX, currentY);
		}
			
		int nextX = (int)currentDirection.y * multiplyer;
		int nextY = (int)currentDirection.x * multiplyer;
		Vector3 newPosition = transform.position + currentDirection * Time.deltaTime * VELOCITY;

		if (currentY + nextY < 0) {
			currentY = MapManager.Instance.cols - 1;
			transform.position = MapManager.Instance.GetWorldPos (currentX, currentY);
		}
		else if (currentY + nextY >= MapManager.Instance.cols) {
			currentY = 0;
			transform.position = MapManager.Instance.GetWorldPos (currentX, currentY);
		}

		if (!MapManager.Instance.IsWalkable (currentX + nextX, currentY + nextY)) {
			transform.position = MapManager.Instance.GetWorldPos (currentX, currentY);		
		}
		else if (MapManager.Instance.IsWalkable(newPosition)) {
			transform.position = newPosition;
		}

		previousDirection = currentDirection;
	}
}

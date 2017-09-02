using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
	// GLOBAL CONSTANTS
	// BULLET
	public static float TIME_BETWEEN_SHOTS = 500.0f;
	public static int BULLET_AMOUNT = 10;
	public static float BULLET_SPEED = 1000.0f;
	public static float BULLET_TTL = TIME_BETWEEN_SHOTS * (BULLET_AMOUNT - 1);
	// CAMERA
	public static float CAMERA_DISTANCE = 10.0f;
	// CHARACTER
	public static int CHARACTER_ROTATION_SPEED = 400;
	public static float CHARACTER_VELOCITY = 40.0f;
	// ZOMBIES
	public static float ZOMBIE_VELOCITY = 10f;
	public static float ZOMBIE_SHOW_BODY = 3000.0f;
	public static int ZOMBIE_AMOUNT = 1;
	public static double ZOMBIE_TIME_BETWEEN_SPAWNS = 5000.0f;
	public static float ZOMBIE_SPAWN_DISTANCE = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {
	// GLOBAL CONSTANTS
	// BULLET
	public static float TIME_BETWEEN_SHOTS = 300.0f;
	public static int BULLET_AMOUNT = 10;
	public static float BULLET_SPEED = 1000.0f;
	public static float BULLET_TTL = TIME_BETWEEN_SHOTS * (BULLET_AMOUNT - 1);
	// CAMERA
	public static float CAMERA_DISTANCE = 10.0f;
	// CHARACTER
	public static int CHARACTER_ROTATION_SPEED = 200;
	public static float CHARACTER_VELOCITY = 800.0f;
	// ZOMBIES
	public static float ZOMBIE_VELOCITY = 100f;
	public static float ZOMBIE_SHOW_BODY = 3000.0f;
	public static int ZOMBIE_AMOUNT = 20;
	public static double ZOMBIE_TIME_BETWEEN_SPAWNS = 1000.0f;
	public static float ZOMBIE_SPAWN_DISTANCE = 10.0f;
	// GAME CONSTANTS
	public static int SCORE_MULTIPLIER = 10;
	public static int ZOMBIE_VELOCITY_MULTIPLIER = 10;
	public static int ZOMBIE_TIME_SPAWN_MULTIPLIER = 10;

	// Game variables
	public Text scoreText;
	int zombiesKilled;

	// Use this for initialization
	void Start () {
		zombiesKilled = 0;
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Score: " + Score ().ToString();
	}

	public void ZombieKilled() {
		zombiesKilled++;
	}

	public int Score() {
		return zombiesKilled * SCORE_MULTIPLIER;
	}

	public float ZombieVelocity() {
		return ZOMBIE_VELOCITY + (ZOMBIE_VELOCITY_MULTIPLIER * zombiesKilled); 
	}

	public double ZombieTimeSpawn() {
		return ZOMBIE_TIME_BETWEEN_SPAWNS - (ZOMBIE_TIME_SPAWN_MULTIPLIER * zombiesKilled);
	}
}

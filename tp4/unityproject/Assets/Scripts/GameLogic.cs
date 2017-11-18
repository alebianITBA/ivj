using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviourSingleton<GameLogic> {
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
	public static float ZOMBIE_SPAWN_DISTANCE = 6.0f;
	// GAME CONSTANTS
	public static int SCORE_MULTIPLIER = 10;
	public static int ZOMBIE_VELOCITY_MULTIPLIER = 5;
	public static int ZOMBIE_TIME_SPAWN_MULTIPLIER = 10;

    public static int PLAYERHEALTH = 100;

	// Game variables
	public Text scoreText;
    public Text lifeText;
    public Character player;
    int zombiesKilled;

	void Initialize () {
		zombiesKilled = 0;
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
		double time = ZOMBIE_TIME_BETWEEN_SPAWNS - (ZOMBIE_TIME_SPAWN_MULTIPLIER * zombiesKilled);
        if (time <= 0)
        {
            return 0.0f;
        }
        return time;
	}

    public int GetCharacterRotationSpeed()
    {
        return CHARACTER_ROTATION_SPEED + (zombiesKilled * 1);
    }

    public float GetCharacterVelocity()
    {
        return (float)(CHARACTER_VELOCITY + (zombiesKilled * 1));
    }
}

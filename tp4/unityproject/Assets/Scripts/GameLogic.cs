using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviourSingleton<GameLogic> {
	// GLOBAL CONSTANTS
	// BULLET
	public static float TIME_BETWEEN_SHOTS = 300.0f;
	public static float TIME_BETWEEN_MELEE = 500.0f;
	public static float BULLET_SPEED = 1000.0f;
	public static int STARTING_BULLETS_AMOUNT = 10;
	public static float BULLET_TTL = TIME_BETWEEN_SHOTS * (STARTING_BULLETS_AMOUNT - 1);
	public static int DEFAULT_AMMO_SPAWNING_PONTS = 2;
	public static int MAX_AMMO = 100;
	public static int SPAWNING_POINTS = 1;
	// CAMERA
	public static float CAMERA_DISTANCE = 10.0f;
	// CHARACTER
	public static int CHARACTER_ROTATION_SPEED = 200;
	public static float CHARACTER_VELOCITY = 500.0f;
	// ZOMBIES
	public static float ZOMBIE_VELOCITY = 200f;
	public static float WARRIOR_SHOW_BODY = 3000.0f;
	public static int WARRIOR_AMOUNT = 100;
	public static double ZOMBIE_TIME_BETWEEN_SPAWNS = 1000.0f;
	public static float WARRIOR_SPAWN_DISTANCE = 6.0f;
	public static int DEFAULT_ZOMBIE_SPAWNING_PONTS = 5;
	// GAME CONSTANTS
	public static int ZOMBIE_KILLED_SCORE = 10;
	public static float ZOMBIE_VELOCITY_MULTIPLIER = 1.5f;
	public static int ZOMBIE_TIME_SPAWN_MULTIPLIER = 10;
    public static int PLAYERHEALTH = 100;
	public static float KNIFE_SPREAD = 45.0f;
	// ACCESSORIES
	public static int HEALTH_KIT_HP = 20;
    public static int SPECIAL_BOX_SCORE = 300;
    public static int AMMO_RELOAD = 5;
	public static float HEALTH_KIT_PROBABILITY = 0.4f;
	public static float SPECIAL_BOX_PROBABILITY = 0.2f;

	[HideInInspector]
    public Character player;

	public void SetPlayer(GameObject player) {
		this.player = player.GetComponent<Character>();
	}

	public float WarriorVelocity() {
		return ZOMBIE_VELOCITY + (ZOMBIE_VELOCITY_MULTIPLIER * ScoreMultiplier()); 
	}

	public void WarriorKilled() {
        player.ZombieKilled ();
	}

	public double ZombieTimeSpawn() {
		double time = ZOMBIE_TIME_BETWEEN_SPAWNS - (ZOMBIE_TIME_SPAWN_MULTIPLIER * ScoreMultiplier());
        if (time <= 0) {
            return 0.0f;
        }
        return time;
	}

    public int GetCharacterRotationSpeed() {
		return CHARACTER_ROTATION_SPEED + (int)ScoreMultiplier();
    }

    public float GetCharacterVelocity() {
		return (float)(CHARACTER_VELOCITY + (int)ScoreMultiplier());
    }

	private float ScoreMultiplier() {
		return player.score / 10f;
	}

    public bool IsPaused() {
        if (player != null) {
            return player.paused;
        }
        return false;
    }
}

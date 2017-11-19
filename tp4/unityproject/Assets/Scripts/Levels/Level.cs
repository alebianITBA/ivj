using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level {
	public enum Tile { Empty, Wall, Floor, OuterWall, PlayerSpawn, ZombieSpawn, AmmoSpawn };
	public enum Direction { North, East, South, West };

	protected Tile[,] map;
	protected List<LevelPosition> zombieSpawns;

	public Level() {
		
	}

	public Level (Tile[,] map)
	{
		this.map = map;
	}

	public Tile[,] GetMap() {
		return map;
	}

	public List<LevelPosition> GetAvailableZombieSpawnPositions() {
		return zombieSpawns;
	}

	public abstract LevelPosition PlayerSpawningPoint(Direction direction);
	public abstract LevelPosition PlayerStartingPoint();

	protected Level.Tile[,] GetRandomMap(int rows, int cols, float wallChance, int seed) {
		Level.Tile[,] map = new Level.Tile[rows, cols];

		UnityEngine.Random.InitState (seed);	
		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				if (UnityEngine.Random.value < wallChance) {
					map [row, col] = Level.Tile.Wall;
				} else {
					map [row, col] = Level.Tile.Floor;
				}
			}
		}

		return map;
	}

	protected Level.Tile[,] GetEmptyMap(int rows, int cols) {
		Level.Tile[,] map = new Level.Tile[rows, cols];

		for (int row = 0; row < map.GetLength(0); row++) {
			for (int col = 0; col < map.GetLength(1); col++) {
				map [row, col] = Level.Tile.Empty;
			}
		}

		return map;
	}

	public void AddZombieSpawningPoints(int amount, LevelPosition playerPosition) {
		List<LevelPosition> availableTiles = GetAvailableTiles();
		availableTiles.RemoveAll (item => playerPosition.Distance(item) < (int)GameLogic.ZOMBIE_SPAWN_DISTANCE);
		zombieSpawns = new List<LevelPosition> ();

		// Take "amount" random tiles from the available if possible
		int toTake = Math.Min(amount, availableTiles.Count);
		while (zombieSpawns.Count < toTake) {
			int rand = UnityEngine.Random.Range(0, availableTiles.Count);
			// Change Tile
			LevelPosition place = availableTiles [rand];
			map [place.x, place.y] = Tile.ZombieSpawn;
			// Add to list
			zombieSpawns.Add (place);
			// Remove from possible
			availableTiles.RemoveAt (rand);
		}
	}

	public void AddAmmo(int amount) {
		List<LevelPosition> availableTiles = GetAvailableTiles();
		// Take "amount" random tiles from the available if possible
		int toTake = Math.Min(amount, availableTiles.Count);
		int taken = 0;
		while (taken < toTake) {
			int rand = UnityEngine.Random.Range(0, availableTiles.Count);
			// Change Tile
			LevelPosition place = availableTiles [rand];
			map [place.x, place.y] = Tile.AmmoSpawn;
			// Remove from possible
			availableTiles.RemoveAt (rand);
			taken++;
		}
	}

	private List<LevelPosition> GetAvailableTiles() {
		if (map == null) {
			throw new OperationCanceledException ("Level not initialised.");
		}
		List<LevelPosition> availableTiles = new List<LevelPosition> ();

		// First check which tiles are available to use
		for (int x = 0; x < map.GetLength (0); x++) {
			for (int y = 0; y < map.GetLength (1); y++) {
				if (map [x, y] == Tile.Floor) {
					availableTiles.Add (new LevelPosition (x, y));
				}
			}
		}

		return availableTiles;
	}
}

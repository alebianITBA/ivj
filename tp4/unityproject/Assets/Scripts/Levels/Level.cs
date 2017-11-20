using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level {
	public enum Tile { Empty, Wall, Floor, OuterWall, PlayerSpawn, ZombieSpawn, AmmoSpawn, HealthKitSpawn, SpecialBoxSpawn };
	//public enum Direction { North, East, South, West };
	public enum Direction { NorthEast, North, NorthWest, East, SouthEast, South, SouthWest, West, Center };

	protected Tile[,] map;
	protected List<LevelPosition> zombieSpawns;
	private HashSet<Warrior> warriors;

    public float renderedMinX;
    public float renderedMaxX;
    public float renderedMinY;
    public float renderedMaxY;

	public Level() {
		this.warriors = new HashSet<Warrior> ();
	}

	public Level (Tile[,] map)
	{
		this.map = map;
		this.warriors = new HashSet<Warrior> ();
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

	public static int GetSeedOffset(Direction dir) {
		switch (dir) {
		case Direction.North:
			return 100;
		case Direction.South:
			return -100;
		case Direction.East:
			return -1;
		case Direction.West:
			return 1;
		case Direction.NorthEast:
			return GetSeedOffset (Direction.North) + GetSeedOffset (Direction.East);
		case Direction.NorthWest:
			return GetSeedOffset (Direction.North) + GetSeedOffset (Direction.West);
		case Direction.SouthEast:
			return GetSeedOffset (Direction.South) + GetSeedOffset (Direction.East);
		case Direction.SouthWest:
			return GetSeedOffset (Direction.South) + GetSeedOffset (Direction.West);
		case Direction.Center:
			return 0;
		}
		return 0;
	}

	public static Vector3 GetPosition(Direction dir) {
		switch (dir) {
		case Direction.North:
			return new Vector3(0,-1,0);
		case Direction.South:
			return new Vector3(0,1,0);
		case Direction.East:
			return new Vector3(-1,0,0);
		case Direction.West:
			return new Vector3(1,0,0);
		case Direction.NorthEast:
			return GetPosition (Direction.North) + GetPosition (Direction.East);
		case Direction.NorthWest:
			return GetPosition (Direction.North) + GetPosition (Direction.West);
		case Direction.SouthEast:
			return GetPosition (Direction.South) + GetPosition (Direction.East);
		case Direction.SouthWest:
			return GetPosition (Direction.South) + GetPosition (Direction.West);
		case Direction.Center:
			return new Vector3 (0, 0, 0);
		}
		return new Vector3 (0, 0, 0);
	}

	public static Direction GetOpposite(Direction dir) {
		switch (dir) {
		case Direction.North:
			return Direction.South;
		case Direction.South:
			return Direction.North;
		case Direction.East:
			return Direction.West;
		case Direction.West:
			return Direction.East;
		case Direction.NorthEast:
			return Direction.SouthWest;
		case Direction.NorthWest:
			return Direction.SouthEast;
		case Direction.SouthEast:
			return Direction.NorthWest;
		case Direction.SouthWest:
			return Direction.NorthEast;
		case Direction.Center:
			return Direction.Center;
		}
		return Direction.Center;
	}

	public static Direction[] GetNeighbours(Direction dir) {
		switch (dir) {
		case Direction.North:
			return new Direction[]{Direction.NorthEast, Direction.NorthWest};
		case Direction.South:
			return new Direction[]{Direction.SouthEast, Direction.SouthWest};
		case Direction.East:
			return new Direction[]{Direction.NorthEast, Direction.SouthEast};
		case Direction.West:
			return new Direction[]{Direction.NorthWest, Direction.SouthWest};
		}
		return new Direction[]{};
	}

	public static Direction GetReplacement(Direction dir, Direction neigh) {
		if (dir == Level.Direction.West || dir == Level.Direction.East) {
			switch (neigh) {
			case Level.Direction.NorthEast:
			case Level.Direction.NorthWest:
				return Level.Direction.North;
			case Level.Direction.SouthEast:
			case Level.Direction.SouthWest:
				return Direction.South;
			}
		} else if (dir == Level.Direction.North || dir == Level.Direction.South) {
			switch (neigh) {
			case Level.Direction.NorthEast:
			case Level.Direction.SouthEast:
				return Level.Direction.East;
			case Level.Direction.NorthWest:
			case Level.Direction.SouthWest:
				return Direction.West;
			}
		}
		return Direction.Center;
	}

	public static Direction GetOpposite(Direction dir, Direction neigh) {
		if (dir == Level.Direction.West || dir == Level.Direction.East) {
			switch (neigh) {
			case Level.Direction.NorthEast:
				return Direction.NorthWest;
			case Level.Direction.NorthWest:
				return Level.Direction.NorthEast;
			case Level.Direction.SouthEast:
				return Direction.SouthWest;
			case Level.Direction.SouthWest:
				return Direction.SouthEast;;
			}
		} else if (dir == Level.Direction.North || dir == Level.Direction.South) {
			switch (neigh) {
			case Level.Direction.NorthEast:
				return Direction.SouthEast;
			case Level.Direction.SouthEast:
				return Level.Direction.NorthEast;
			case Level.Direction.NorthWest:
				return Direction.SouthWest;
			case Level.Direction.SouthWest:
				return Direction.NorthWest;
			}
		}
		return Direction.Center;
	}

	public void AddZombieSpawningPoints(int amount, LevelPosition playerPosition) {
		List<LevelPosition> availableTiles = GetAvailableTiles();
		availableTiles.RemoveAll (item => playerPosition.Distance(item) < (int)GameLogic.WARRIOR_SPAWN_DISTANCE);
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

	public void AddHealthKit() {
		if (UnityEngine.Random.value < GameLogic.HEALTH_KIT_PROBABILITY) {
			List<LevelPosition> availableTiles = GetAvailableTiles ();
			int rand = UnityEngine.Random.Range(0, availableTiles.Count);
			LevelPosition place = availableTiles [rand];
			map [place.x, place.y] = Level.Tile.HealthKitSpawn;
		}
	}

	public void AddSpecialBox() {
		if (UnityEngine.Random.value < GameLogic.SPECIAL_BOX_PROBABILITY) {
			List<LevelPosition> availableTiles = GetAvailableTiles ();
			int rand = UnityEngine.Random.Range(0, availableTiles.Count);
			LevelPosition place = availableTiles [rand];
			map [place.x, place.y] = Level.Tile.SpecialBoxSpawn;
		}
	}

	protected List<LevelPosition> GetAvailableTiles() {
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

	public void AddWarrior(Warrior warrior) {
		warriors.Add (warrior);
	}

	public void RemoveWarrior(Warrior warrior) {
		warriors.Remove (warrior);
	}

	public void recycleWarriors() {
		List<Warrior> wrs = new List<Warrior> (warriors);
		foreach (Warrior w in wrs) {
			warriors.Remove (w);
			WarriorManager.Instance.RecycleWarrior (w);
		}
	}

	public HashSet<Warrior> GetWarriors() {
		return warriors;
	}
}

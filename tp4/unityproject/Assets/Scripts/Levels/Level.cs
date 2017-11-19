using System;
using UnityEngine;

public abstract class Level {
	public enum Tile { Empty, Wall, Floor, OuterWall, PlayerSpawn, ZombieSpawn };
	//public enum Direction { North, East, South, West };
	public enum Direction { NorthEast, North, NorthWest, East, SouthEast, South, SouthWest, West, Center };

	protected Tile[,] map;

	public Level() {
		
	}

	public Level (Tile[,] map)
	{
		this.map = map;
	}

	public Tile[,] GetMap() {
		return map;
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
}

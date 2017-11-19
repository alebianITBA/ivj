using System;

public abstract class Level {
	public enum Tile { Empty, Wall, Floor, OuterWall, PlayerSpawn, ZombieSpawn };
	public enum Direction { North, East, South, West };

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
}

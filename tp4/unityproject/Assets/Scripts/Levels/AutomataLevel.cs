using System;

public class AutomataLevel : Level {
	public AutomataLevel (int rows, int cols, int initialRounds, int afterRounds, int birth, int death, int afterBirth, int afterDeath, float initialWallChance) : base() {
		this.map = GenerateMap (rows, cols, initialRounds, afterRounds, birth, death, afterBirth, afterDeath, initialWallChance);
	}

	private Level.Tile[,] GenerateMap(int rows, int cols, int initialRounds, int afterRounds, int initialBirth, int initialDeath, int afterBirth, int afterDeath, float initialWallChance) {
		Level.Tile[,] map = GetRandomMap(rows, cols, initialWallChance);

		for (int i = 0; i < initialRounds; i++) {
			Level.Tile[,] newMap = new Level.Tile[rows, cols];
			for (int row = 0; row < map.GetLength(0); row++) {
				for (int col = 0; col < map.GetLength(1); col++) {
					int numberOfWalls = NumberOfWallsAround (map, row, col);
					if (map[row, col] == Level.Tile.Wall) {
						if (numberOfWalls < initialBirth) {
							newMap [row, col] = Level.Tile.Wall;
						} else {
							newMap [row, col] = Level.Tile.Floor;
						}
					} else {
						if (numberOfWalls > initialDeath) {
							newMap [row, col] = Level.Tile.Wall;
						} else {
							newMap [row, col] = Level.Tile.Floor;
						}
					}
				}
			}
			map = newMap;
		}

		for (int i = 0; i < afterRounds; i++) {
			Level.Tile[,] newMap = new Level.Tile[rows, cols];
			for (int row = 0; row < map.GetLength(0); row++) {
				for (int col = 0; col < map.GetLength(1); col++) {
					int numberOfWalls = NumberOfWallsAround (map, row, col);
					if (map[row, col] == Level.Tile.Wall) {
						if (numberOfWalls < afterBirth) {
							newMap [row, col] = Level.Tile.Wall;
						} else {
							newMap [row, col] = Level.Tile.Floor;
						}
					} else {
						if (numberOfWalls > afterDeath) {
							newMap [row, col] = Level.Tile.Wall;
						} else {
							newMap [row, col] = Level.Tile.Floor;
						}
					}
				}
			}
			map = newMap;
		}

		return map;
	}

	private Level.Tile[,] GetRandomMap(int rows, int cols, float wallChance) {
		Level.Tile[,] map = new Level.Tile[rows, cols];

		Level.Tile[] availableTiles = new Level.Tile[] { Level.Tile.Floor, Level.Tile.Wall };

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

	private int NumberOfWallsAround(Level.Tile[,] map, int x, int y) {
		int count = 0;
		for(int i = -1; i < 2; i++) {
			for(int j = -1; j < 2; j++) {
				int neighbour_x = x + i;
				int neighbour_y = y + j;
				if(i == 0 && j == 0) {
				}
				else if(neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= map.GetLength(0) || neighbour_y >= map.GetLength(1)) {
					count = count + 1;
				}
				else if(map[neighbour_x, neighbour_y] == Level.Tile.Wall) {
					count = count + 1;
				}
			}
		}
		return count;
	}
}

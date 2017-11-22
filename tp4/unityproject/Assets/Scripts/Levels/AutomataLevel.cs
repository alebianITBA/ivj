using System;
using System.Collections.Generic;
using UnityEngine;

public class AutomataLevel : Level {
	private Dictionary<Direction, LevelPosition> playerSpawningPositions;

	public AutomataLevel (int rows, int cols, int initialRounds, int afterRounds, int birth, int death, int afterBirth, int afterDeath, float initialWallChance, int seed) : base() {
		this.map = GenerateMap (rows, cols, initialRounds, afterRounds, birth, death, afterBirth, afterDeath, initialWallChance, seed);
		CalculatePlayerSpawningPositions ();
	}

	public override LevelPosition PlayerStartingPoint() {
		return PlayerSpawningPoint (Direction.South);
	}

	public override LevelPosition PlayerSpawningPoint(Direction direction) {
        for (int x = 0; x < map.GetLength (0); x++) {
            for (int y = 0; y < map.GetLength (1); y++) {
                if (map[x, y] == Tile.Floor) {
                    return new LevelPosition (x, y);
                }
            }
        }
        return new LevelPosition (-1, -1);
	}

	private void CalculatePlayerSpawningPositions() {
		this.playerSpawningPositions = new Dictionary<Direction, LevelPosition> ();
		playerSpawningPositions.Add (Direction.North, new LevelPosition (map.GetLength(0) - 1, map.GetLength(1) - 1));
		playerSpawningPositions.Add (Direction.South, new LevelPosition (0, 0));
		playerSpawningPositions.Add (Direction.West, new LevelPosition (0, map.GetLength(1) - 1));
		playerSpawningPositions.Add (Direction.East, new LevelPosition (map.GetLength(0) - 1, 0));
	}

	private Level.Tile[,] GenerateMap(int rows, int cols, int initialRounds, int afterRounds, int initialBirth, int initialDeath, int afterBirth, int afterDeath, float initialWallChance, int seed) {
//		bool insertHealth = UnityEngine.Random.value < GameLogic.HEALTH_KIT_PROBABILITY;
//		bool insertBox = UnityEngine.Random.value < GameLogic.SPECIAL_BOX_PROBABILITY;

		Level.Tile[,] map = GetRandomMap(rows, cols, initialWallChance, seed);

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

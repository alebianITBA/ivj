using System;

public class RoomLevel : Level {
	public RoomLevel (int rows, int cols) : base() {
		this.map = GenerateMap (rows, cols);
	}

	public override LevelPosition PlayerSpawningPoint(Direction direction) {
		// TODO: Return the same spawning point for every direction
		return new LevelPosition (1, 1);
	}

	private Level.Tile[,] GenerateMap(int rows, int cols) {
		Level.Tile[,] map = GetEmptyMap(rows, cols);

		return map;
	}
}


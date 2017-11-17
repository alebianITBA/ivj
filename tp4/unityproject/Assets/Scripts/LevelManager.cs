using UnityEngine;

public class LevelManager : MonoBehaviourSingleton<LevelManager> {
	public Level CreateLevel(int cols, int rows) {
		Level.Tile[,] map = new Level.Tile[6, 5];
		map [0, 0] = Level.Tile.Floor;
		map [0, 1] = Level.Tile.Floor;
		map [0, 2] = Level.Tile.Floor;
		map [0, 3] = Level.Tile.Floor;
		map [0, 4] = Level.Tile.Floor;
		map [1, 0] = Level.Tile.Floor;
		map [1, 1] = Level.Tile.Floor;
		map [1, 2] = Level.Tile.Floor;
		map [1, 3] = Level.Tile.Floor;
		map [1, 4] = Level.Tile.Floor;
		map [2, 0] = Level.Tile.Floor;
		map [2, 1] = Level.Tile.Floor;
		map [2, 2] = Level.Tile.Floor;
		map [2, 3] = Level.Tile.Floor;
		map [2, 4] = Level.Tile.Floor;
		map [3, 0] = Level.Tile.Floor;
		map [3, 1] = Level.Tile.Floor;
		map [3, 2] = Level.Tile.Floor;
		map [3, 3] = Level.Tile.Floor;
		map [3, 4] = Level.Tile.Floor;
		map [4, 0] = Level.Tile.Floor;
		map [4, 1] = Level.Tile.Floor;
		map [4, 2] = Level.Tile.Floor;
		map [4, 3] = Level.Tile.Floor;
		map [4, 4] = Level.Tile.Floor;
		map [5, 0] = Level.Tile.Floor;
		map [5, 1] = Level.Tile.Floor;
		map [5, 2] = Level.Tile.Floor;
		map [5, 3] = Level.Tile.Floor;
		map [5, 4] = Level.Tile.Floor;

		return new Level(map);
	}

	public Level GetAutomataLevel(int rows, int cols, int initialRounds, int afterRounds, int birth, int death, int afterBirth, int afterDeath, float initialWallChance) {
		return new AutomataLevel (rows, cols, initialRounds, afterRounds, birth, death, afterBirth, afterDeath, initialWallChance);
	}
}

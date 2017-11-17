using System;

public class Level
{
	public enum LevelType { Automata };
	public enum Tile { Empty, Wall, Floor };

	public Tile[,] map;

	public Level() {
		
	}

	public Level (Tile[,] map)
	{
		this.map = map;
	}
}

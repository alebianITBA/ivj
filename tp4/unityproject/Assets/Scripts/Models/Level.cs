using System;

public class Level
{
	public enum Tile { Empty, Wall, Floor };

	public Tile[,] map;

	public Level (Tile[,] map)
	{
		this.map = map;
	}
}

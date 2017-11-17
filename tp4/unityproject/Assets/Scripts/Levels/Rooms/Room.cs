using System;

public abstract class Room {
	public abstract Level.Tile[,] BluePrint();
	public abstract int Width ();
	public abstract int Height ();
}

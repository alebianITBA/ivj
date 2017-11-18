using System;

public abstract class Room {
	public abstract Level.Tile[,] Blueprint();
	public abstract int Width ();
	public abstract int Height ();
}

using System;

public class LevelPosition {
	public int x { get; }
	public int y { get; }

	public LevelPosition(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public int Distance(LevelPosition other) {
		return Math.Abs (this.x - other.x) + Math.Abs (this.y - other.y);
	}

    public override string ToString ()
    {
        return string.Format ("[LevelPosition: x={0}, y={1}]", x, y);
    }
}

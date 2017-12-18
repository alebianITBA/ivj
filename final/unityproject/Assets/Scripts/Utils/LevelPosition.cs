using System;
using UnityEngine;

public class LevelPosition
{
    public int x { get; }

    public int y { get; }

    public LevelPosition (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int Distance (LevelPosition other)
    {
        return Math.Abs(this.x - other.x) + Math.Abs(this.y - other.y);
    }

    public override string ToString ()
    {
        return string.Format("[LevelPosition: x={0}, y={1}]", x, y);
    }

    public Vector3 GetCoordinates ()
    {
        return new Vector3((x * Drawer.Instance.tileLength), (y * Drawer.Instance.tileLength), 0);
    }

    public Vector3 GetHalfCoordinates ()
    {
        return new Vector3((x * Drawer.Instance.tileLength) + Drawer.Instance.halfTileLength, (y * Drawer.Instance.tileLength) + Drawer.Instance.halfTileLength, 0);
    }
}

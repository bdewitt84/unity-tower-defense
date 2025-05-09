using UnityEngine;

public readonly struct GridCoordinate
{
    public int X { get; }
    public int Y { get; }

    public GridCoordinate(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    } 

}



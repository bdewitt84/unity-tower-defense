using System.Collections.Generic;
using UnityEngine;

public abstract class GridMap
{
    protected int width;
    protected int height;

    public int Width => width;
    public int Height => height;

    public GridMap(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public IEnumerable<GridCoordinate> GetNeighbors(GridCoordinate gridCoordinate)
    {
        int x = gridCoordinate.X;
        int y = gridCoordinate.Y;

        // Ordered N, E, S, W
        if (IsWithinBounds(x, y + 1)) yield return new GridCoordinate(x, y + 1);
        if (IsWithinBounds(x + 1, y)) yield return new GridCoordinate(x + 1, y);
        if (IsWithinBounds(x, y - 1)) yield return new GridCoordinate(x, y - 1);
        if (IsWithinBounds(x - 1, y)) yield return new GridCoordinate(x - 1, y);
    }
}



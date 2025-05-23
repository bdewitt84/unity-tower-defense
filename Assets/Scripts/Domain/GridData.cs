using System;

public class GridData : GridMap
{
    private GridCell[,] gridCells;

    public GridData(int width, int height) : base(width, height)
    {
        gridCells = new GridCell[width, height];
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                gridCells[x, y] = new GridCell(GridCell.CellType.Buildable);
            }
        }
    }

    public GridCell GetCell(int x, int y)
    {
        if (IsWithinBounds(x, y))
        {
            return gridCells[x, y];
        }
        throw new IndexOutOfRangeException("Cell location is out of bounds");
    }

    public GridCell GetCell(GridCoordinate coord)
    {
        return GetCell(coord.X, coord.Y);
    }

    public void SetCellType(int x, int y, GridCell.CellType type)
    {
        gridCells[x, y].Type = type;
    }

    public void SetCellOccupied(int x, int y, bool occupied)
    {
        gridCells[x,y].isOccupied = occupied;
    }
}

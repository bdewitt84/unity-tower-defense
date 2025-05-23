using UnityEngine;

public struct GridCell
{
    public enum CellType { Buildable, Path }
    public CellType Type;
    public bool isOccupied;

    public GridCell(CellType type, bool isOccupied = false)
    {
        this.Type = type;
        this.isOccupied = isOccupied;
    }
}

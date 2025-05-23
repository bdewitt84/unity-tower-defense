// ./Assets/Scripts/GameBoard.cs

using UnityEngine;

// Author: Brett DeWitt
// 
// Created: 5/8/2025
// 
// Description:
//   Stores board state and handles tower instantiation based on
//   TowerPlacementRequest event


public class GameBoardController : MonoBehaviour
{
    private enum CellState { Empty, Blocked }

    [SerializeField] private int width = 22;
    [SerializeField] private int height = 22;
    [SerializeField] private float cellSize = 1.0f;

    private GridData board;

    private void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        board = new GridData(width, height);
    }

    public bool CanPlaceTower(Vector3 worldPosition)
    {
        GridCoordinate gridCoord = GetGridCoordinate(worldPosition);
        return CanPlaceTower(gridCoord);
    }

    public bool CanPlaceTower(GridCoordinate gridCoord)
    {
        if (!board.IsWithinBounds(gridCoord.X, gridCoord.Y)
            || board.GetCell(gridCoord.X, gridCoord.Y).isOccupied)
        {
            return false;
        }
        return true;
    }

    public void PlaceTower(Vector3 globalPosition, GameObject towerInstance)
    {
        GridCoordinate girdCoord = GetGridCoordinate(globalPosition);
        board.SetCellOccupied(girdCoord.X, girdCoord.Y, true);

        globalPosition = GetWorldPositionFromGridCoordinates(girdCoord);
        Vector3 towerOffset = new Vector3(0.5f, 0.0f, 0.5f);
        globalPosition += towerOffset;
        towerInstance.transform.position = globalPosition;
    }

    // Retrurns the grid coordinates corresponding to the global Vector3 position
    private GridCoordinate GetGridCoordinate(Vector3 worldPosition)
    {
        int coord_x = Mathf.FloorToInt(worldPosition.x + (width * cellSize) / 2);
        int coord_y = Mathf.FloorToInt(worldPosition.z + (height * cellSize) / 2);
        return new GridCoordinate(coord_x, coord_y);
    }

    private Vector3 GetWorldPositionFromGridCoordinates(GridCoordinate gridCoordinate)
    {
        float world_x = gridCoordinate.X * cellSize - (width * cellSize) / 2;
        float world_y = gridCoordinate.Y * cellSize - (height * cellSize) / 2;
        Vector3 worldPosition = new Vector3(world_x, transform.position.y, world_y);
        return worldPosition;
    }
}

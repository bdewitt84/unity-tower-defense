// ./Assets/Scripts/GameBoard.cs

using System;
using UnityEngine;

// Author: Brett DeWitt
// 
// Created: 5/8/2025
// 
// Description:
//   Stores board state and handles tower placement


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

    // Returns true if a tower can be placed at the board's grid coordinates corresponding to worldPosition
    public bool CanPlaceTower(Vector3 worldPosition)
    {
        GridCoordinate gridCoord = GetGridCoordinate(worldPosition);
        return CanPlaceTower(gridCoord);
    }

    // Returns true if a tower can be placed at the board's grid coordinates
    public bool CanPlaceTower(GridCoordinate gridCoord)
    {
        if (!board.IsWithinBounds(gridCoord.X, gridCoord.Y)
            || board.GetCell(gridCoord.X, gridCoord.Y).isOccupied)
        {
            return false;
        }
        return true;
    }

    // Places instance of tower in center of cell corresponding to globalPosition
    public void PlaceTower(Vector3 globalPosition, GameObject towerInstance)
    {
        GridCoordinate gridCoord = GetGridCoordinate(globalPosition);
        board.SetCellOccupied(gridCoord.X, gridCoord.Y, true);

        globalPosition = SnapToGrid(globalPosition);
        towerInstance.transform.position = globalPosition;
    }


    // Retrurns the grid coordinates corresponding to the global Vector3 position
    public GridCoordinate GetGridCoordinate(Vector3 worldPosition)
    {
        int coord_x = Mathf.FloorToInt(worldPosition.x + (width * cellSize) / 2);
        int coord_y = Mathf.FloorToInt(worldPosition.z + (height * cellSize) / 2);
        return new GridCoordinate(coord_x, coord_y);
    }

    // Returns world position corresponding to origin of the provided grid coordinates
    public Vector3 GetWorldPositionFromGridCoordinates(GridCoordinate gridCoordinate)
    {
        float world_x = gridCoordinate.X * cellSize - (width * cellSize) / 2;
        float world_y = gridCoordinate.Y * cellSize - (height * cellSize) / 2;
        Vector3 worldPosition = new Vector3(world_x, transform.position.y, world_y);
        return worldPosition;
    }

    // Returns position in the center of the cell at worldPosition
    public Vector3 SnapToGrid(Vector3 worldPosition)
    {
        GridCoordinate gridCoord = GetGridCoordinate(worldPosition);
        worldPosition = GetWorldPositionFromGridCoordinates(gridCoord);
        Vector3 offset = new Vector3(cellSize/2, 0.0f, cellSize/2);
        return worldPosition += offset;
    }
}

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

    [SerializeField] private Vector3 offsetFromOrigin = new(-11.0f, 0.0f, -11.0f); // make a function for this
    [SerializeField] private GameObject towerPrefab; // marked for deletion

    private GridData board;

    private void Start()
    {
        InitializeBoard();
    }

    private void OnEnable()
    {
        GameEvents.OnTowerPlacementRequest += HandleTowerPlacementRequest;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerPlacementRequest -= HandleTowerPlacementRequest;
    }

    private void InitializeBoard()
    {
        board = new GridData(width, height);
    }

    private void HandleTowerPlacementRequest(Vector3 globalPosition)
    {
        GridCoordinate gridCoordinate = GetGridCoordinate(globalPosition);
        string reason;
        if (CanPlaceTower(gridCoordinate, out reason))
        {
            Vector3 placeAt = GetWorldPositionFromGridCoordinates(gridCoordinate);
            Vector3 towerOffset = new Vector3(0.5f, 0.0f, 0.5f);
            placeAt += towerOffset;
            GameObject towerObject = InstantiateTower(placeAt);
            TowerController towerController = towerObject.GetComponent<TowerController>();
            board.SetCellOccupied(gridCoordinate.X, gridCoordinate.Y, true);
            GameEvents.TowerPlacementSuccess(towerController);
        }
        else
        {
            GameEvents.TowerPlacementInvalid(gridCoordinate.X, gridCoordinate.Y);
            Debug.Log(reason);
        }
    }

    public void PlaceTower(Vector3 globalPosition, TowerController tower)
    {
        GridCoordinate cell = GetGridCoordinate(globalPosition);
        globalPosition = GetWorldPositionFromGridCoordinates(cell);
        Vector3 towerOffset = new Vector3(0.5f, 0.0f, 0.5f);
        globalPosition += towerOffset;
        GameObject towerObject = InstantiateTower(globalPosition);
        TowerController towerController = towerObject.GetComponent<TowerController>();

        board.SetCellOccupied(cell.X, cell.Y, true);
        GameEvents.TowerPlacementSuccess(towerController);
    }


    private GameObject InstantiateTower(Vector3 globalPosition)
    {
        GameObject tower = Instantiate(towerPrefab, globalPosition, Quaternion.identity);
        return tower;
    }

    public bool CanPlaceTower(Vector3 worldPosition, out string reason)
    {
        GridCoordinate cell = GetGridCoordinate(worldPosition);
        return CanPlaceTower(cell, out reason);
    }

    public bool CanPlaceTower(GridCoordinate gridCoordinate, out string reason)
    {
        if (!board.IsWithinBounds(gridCoordinate.X, gridCoordinate.Y))
        {
            reason = $"Tower placement coordinates out of bounds: {gridCoordinate}";
            return false;
        }
        if (board.GetCell(gridCoordinate.X, gridCoordinate.Y).isOccupied)
        {
            reason = $"Cell already occupied: {gridCoordinate}";
            return false;
        }
        reason = null;
        return true;
    }


    // Retrurns the grid coordinates corresponding to the global Vector3 position
    private GridCoordinate GetGridCoordinate(Vector3 worldPosition)
    {
        //worldPosition -= offsetFromOrigin;
        //int gridX = Mathf.FloorToInt(worldPosition.x);
        //int gridY = Mathf.FloorToInt(worldPosition.z);
        //return new GridCoordinate(gridX, gridY);
        
        int coord_x = Mathf.FloorToInt(worldPosition.x + (width * cellSize) / 2);
        int coord_y = Mathf.FloorToInt(worldPosition.z + (height * cellSize) / 2);
        return new GridCoordinate(coord_x, coord_y);
    }

    private Vector3 GetWorldPositionFromGridCoordinates(GridCoordinate gridCoordinate)
    {
        Vector3 worldPosition = new Vector3(gridCoordinate.X, transform.position.y, gridCoordinate.Y);
        worldPosition += offsetFromOrigin;
        return worldPosition;
    }
}

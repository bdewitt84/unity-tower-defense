using UnityEngine;

public class GameBoardController : MonoBehaviour
{
    private enum CellState { Empty, Blocked }

    [SerializeField] private int width = 22;
    [SerializeField] private int height = 22;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Vector3 offsetFromOrigin = new(-11.0f, 0.0f, -11.0f);

    private GameBoard board;

    private void Start()
    {
        board = new GameBoard(width, height);
    }

    private void OnEnable()
    {
        GameEvents.OnTowerPlacementRequest += HandleTowerPlacementRequest;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerPlacementRequest -= HandleTowerPlacementRequest;
    }

    private void HandleTowerPlacementRequest(Vector3 globalPosition)
    {
        GridCoordinate gridCoordinate = GetGridCoordinateFromWorldPosition(globalPosition);
        string reason;
        if (CanPlaceTower(gridCoordinate, out reason))
        {
            Vector3 placeAt = GetWorldPositionFromGridCoordinates(gridCoordinate);
            Vector3 towerOffset = new Vector3(0.5f, 0.0f, 0.5f);
            placeAt += towerOffset;
            InstantiateTower(placeAt);
            board.SetCellBlocked(gridCoordinate.X, gridCoordinate.Y);
        }
        else
        {
            GameEvents.TowerPlacementInvalid(gridCoordinate.X, gridCoordinate.Y);
            Debug.LogWarning(reason);
        }
    }


    private void InstantiateTower(Vector3 globalPosition)
    {
        GameObject tower = Instantiate(towerPrefab, globalPosition, Quaternion.identity);
    }

    private bool CanPlaceTower(GridCoordinate gridCoordinate, out string reason)
    {
        if (!board.IsWithinBounds(gridCoordinate.X, gridCoordinate.Y))
        {
            reason = $"Tower placement coordinates out of bounds: {gridCoordinate}";
            return false;
        }
        if (!board.IsCellEmpty(gridCoordinate.X, gridCoordinate.Y))
        {
            reason = $"Cell already occupied: {gridCoordinate}";
            return false;
        }
        reason = null;
        return true;
    }


    // Retrurns the grid coordinates corresponding to the global Vector3 position
    private GridCoordinate GetGridCoordinateFromWorldPosition(Vector3 worldPosition)
    {
        worldPosition -= offsetFromOrigin;
        int gridX = Mathf.FloorToInt(worldPosition.x);
        int gridY = Mathf.FloorToInt(worldPosition.z);
        return new GridCoordinate(gridX, gridY);
    }

    private Vector3 GetWorldPositionFromGridCoordinates(GridCoordinate gridCoordinate)
    {
        Vector3 worldPosition = new Vector3(gridCoordinate.X, transform.position.y, gridCoordinate.Y);
        worldPosition += offsetFromOrigin;
        return worldPosition;
    }
}

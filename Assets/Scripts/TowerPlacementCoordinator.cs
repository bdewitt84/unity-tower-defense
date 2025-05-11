using UnityEngine;

public class TowerPlacementCoordinator : MonoBehaviour
{
    [SerializeField] private GameObject gameBoard;
    [SerializeField] private GameObject gameState;

    private GameBoardController gameBoardController;
    private GameStateManager gameStateManager;

    public void Start()
    {
        gameBoardController = gameBoard.GetComponent<GameBoardController>();
        gameStateManager = gameState.GetComponent<GameStateManager>();
    }

    public bool TryPlaceTower(Vector3 worldPosition, TowerController tower)
    {
        string reason;
        if (!gameBoardController.CanPlaceTower(worldPosition, out reason))
        {
            Debug.Log(reason);
            return false;
        }
        if (gameStateManager.GetPlayerGold() < tower.GetCost())
        {
            Debug.Log("Not enough gold");
            return false;
        }

        gameBoardController.PlaceTower(worldPosition, tower);
        return true;
    }
}

using UnityEngine;

// Author: Brett DeWitt
//
// Created: 5.8.25
//
// Description:
//   Cooridnates with GameStateManager and GameBoardController to make sure
//   that the requestion position on the game board is empty and that the
//   player has enough gold to purchase the tower

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
            Debug.Log("Placement blocked: " + reason);
            GameEvents.TowerPlacementInvalid((int)worldPosition.x, (int)worldPosition.z);
            return false;
        }

        if (gameStateManager.GetPlayerGold() < tower.GetCost())
        {
            Debug.Log("Not enough gold");
            GameEvents.TowerPlacementInvalid((int)worldPosition.x, (int)worldPosition.z);
            return false;
        }

        gameBoardController.PlaceTower(worldPosition, tower);
        return true;
    }
}

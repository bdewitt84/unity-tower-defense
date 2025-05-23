// ./Assets/Scripts/Managers/TowerRequestValidator.cs

using UnityEngine;

// Author: Brett DeWitt
//
// Date: 5.23.2025
//
// Description:
//  Listens for a TowerPlacementRequest. If the request is valid, fires
//  TowerPlacementExecute event.

public class TowerRequestValidator : MonoBehaviour
{
    [SerializeField] private GameStateManager _gameStateManager;
    [SerializeField] private GameBoardController _boardController;

    private void OnEnable()
    {
        GameEvents.OnTowerPlacementRequest += HandleTowerPlacementRequest;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerPlacementRequest -= HandleTowerPlacementRequest;
    }

    private void HandleTowerPlacementRequest(Vector3 worldPosition, GameObject towerPrefab)
    {
        TowerController towerController = towerPrefab.GetComponent<TowerController>();
        // check gold
        if (_boardController.CanPlaceTower(worldPosition)
            && _gameStateManager.GetPlayerGold() >= towerController.GetCost())
        {
            Debug.Log("Tower request validated");
            GameEvents.OnTowerPlacementExecute(worldPosition, towerPrefab);
        } else
        {
            Debug.Log("Tower request denied");
            GameEvents.TowerPlacementInvalid(0, 0);
        }
    }
}

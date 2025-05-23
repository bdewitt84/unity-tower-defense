// ./Assets/Scripts/Managers/TowerRequestExecutor.cs

using UnityEngine;

// Auther: Brett DeWitt
//
// Date: 05.23.2025
//
// Description:
//  Listens for TowerRequestExecute, then instantiates a tower and hands it off
//  to GameBoard with a location for placement.

public class TowerRequestExecutor : MonoBehaviour
{

    [SerializeField] private GameBoardController _gameBoardController;

    private void OnEnable()
    {
        GameEvents.OnTowerPlacementExecute += HandleTowerPlacementExecute;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerPlacementExecute -= HandleTowerPlacementExecute;
    }

    private void HandleTowerPlacementExecute(Vector3 worldPosition, GameObject towerPrefab)
    {
        GameObject towerInstance = InstantiateTower(towerPrefab);
        _gameBoardController.PlaceTower(worldPosition, towerInstance);
        GameEvents.TowerPlacementSuccess(towerInstance);
    }

    private GameObject InstantiateTower(GameObject towerPrefab)
    {
        return GameObject.Instantiate(towerPrefab, Vector3.zero, Quaternion.identity);
    }
}

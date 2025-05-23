// ./Assets/Scripts/InputManager.cs

using UnityEngine;

// Author: Brett DeWitt, Dante Borden
//
// Created: 5.22.2025
//
// Description:
//  Handles user input and delegates the associated actions
//
// Notes:
//  Example event pipeline for placing a tower
//  InputManager
//      gets click -> checks if user clicked ground -> fire TowerPlacementRequest event
//  TowerPlacementValidator
//      gets TowerPlacementRequest event -> validates placement -> fires TowerPlacementExecute event
//  TowerPlacementExecutor
//      gets TowerPlacementExecute event -> instantiates tower -> places tower -> fires OnTowerPlaced event


public class InputManager : MonoBehaviour
{

    [SerializeField] private GameObject towerPrefab;


    private void OnEnable()
    {
        GameEvents.OnTowerSelected += HandleTowerSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerSelected -= HandleTowerSelected;
    }

    private void HandleTowerSelected(GameObject towerPrefab)
    {
        this.towerPrefab = towerPrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (LeftMouseButtonIsDown()
            && CameraIsNotNull()
            && GetRaycastHit(out RaycastHit hit)
            )
        {
            if(HitIsGround(hit)
                && TowerIsSelected())
            {
                RequestTowerPlacement(hit);
            }
        } else if (RightMouseButtonIsDown())
        {
            DeselectTower();
        }
    }

    public void SelectTower(GameObject towerPrefab)
    {
        if (towerPrefab == null)
        {
            Debug.Log("Please don't pass null to SelectTower");
            return;
        }
        this.towerPrefab = towerPrefab;
    }

    private void RequestTowerPlacement(RaycastHit hit)
    {
        Vector3 placeAt = hit.point;
        GameEvents.TowerPlacementRequest(placeAt, towerPrefab);
    }

    private bool TowerIsSelected()
    {
        return towerPrefab != null;
    }

    private void DeselectTower()
    {
        towerPrefab = null;
    }

    private bool HitIsGround(RaycastHit hit)
    {
        return hit.transform != null && hit.transform.CompareTag("Ground");
    }

    private bool GetRaycastHit(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit);
    }

    private bool CameraIsNotNull()
    {
        return Camera.main != null;
    }

    private bool LeftMouseButtonIsDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool RightMouseButtonIsDown()
    {
        return Input.GetMouseButtonDown(1);
    }

}

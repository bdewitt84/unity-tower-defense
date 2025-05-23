// ./Assets/Scripts/InputManager.cs

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
//      gets TowerPlacementExecute event -> deducts gold -> places tower -> fires OnTowerPlaced event


using UnityEngine;

public class InputManager : MonoBehaviour
{

    [SerializeField] private GameObject towerPrefab;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LeftMouseButtonIsDown()
            && CameraIsNotNull()
            && GetRaycastHit(out RaycastHit hit)
            )
        {
            if(HitIsGround(hit))
            {
                RequestTowerPlacement(hit);
                Debug.Log("Tower placement requested");
            }
        }
    }

    private void RequestTowerPlacement(RaycastHit hit)
    {
        Vector3 placeAt = hit.point;
        GameEvents.TowerPlacementRequest(placeAt, towerPrefab);
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

}

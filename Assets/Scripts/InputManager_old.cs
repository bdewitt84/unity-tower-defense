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


public class InputManager_old : MonoBehaviour
{

    private GameObject towerPrefab;
    private RaycastHit _lastHit;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _pathLayer;

    private void OnEnable()
    {
        GameEvents.OnTowerSelected += HandleTowerSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnTowerSelected -= HandleTowerSelected;
    }

    // Update is called once per frame
    void Update()
    {
        // Update hit
        if (CameraIsNotNull()
            && GetRaycastHit(out RaycastHit hit))
        {
            // Left click
            if (LeftMouseButtonIsDown()
                && HitIsGround(hit)
                && TowerIsSelected())
            {
                RequestTowerPlacement(hit);
            }
            // Right click
            else if (RightMouseButtonIsDown())
            {
                DeselectTower();
            }
            // Hover
            else if (TowerIsSelected()
                && HitIsGround(hit))
            {
                if (!hit.Equals(_lastHit))
                {
                    RequestTowerPreview(hit);
                }
                _lastHit = hit;
            }
            else
            {
                DisableTowerPreview();
            }   
        }
    }

    private void DisableTowerPreview()
    {
        GameEvents.TowerPreviewDisable();
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

    private void RequestTowerPreview(RaycastHit hit)
    {
        Vector3 previewAt = hit.point;
        GameEvents.TowerPreviewRequest(previewAt, towerPrefab);
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
        return Physics.Raycast(ray, out hit, 100f, _groundLayer | _pathLayer);
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

    private void HandleTowerSelected(GameObject towerPrefab)
    {
        this.towerPrefab = towerPrefab;
    }

}

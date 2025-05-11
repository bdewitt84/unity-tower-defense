using UnityEngine;

// Author: Dante Borden
//
// Created: 04.27.2025
//
// Description:
//
// Handles player-controlled tower placement in the game world.
// Casts a ray from the mouse position to detect valid ground areas for tower spawning.
//
// Modified:
//   04.27.25 Brett DeWitt
//   fixed typos, unresolved references, corrected code
//   05.03.25 Brett DeWitt
//   refactored for readability

public class TowerPlacement : MonoBehaviour
{
    private Vector3 place;
    public GameObject towerPlacementCoordinator;
    private TowerPlacementCoordinator TPCController;
    public GameObject towerPrefab;

    private RaycastHit _hit;

    public bool placing;
    private Ray ray;

    private void Start()
    {
        TPCController = towerPlacementCoordinator.GetComponent<TowerPlacementCoordinator>();
    }

    private void Update()
    {
        if (left_mouse_button_is_down())
        {
            if (camera_is_not_null())
            {
                get_location_of_click();
                if (location_of_click_is_ground())
                {
                    place_tower();
                }
            }
        }
    }

    private void place_tower()
    {
        TowerController tower = towerPrefab.GetComponent<TowerController>();
        TPCController.TryPlaceTower(_hit.point, tower);
    }

    private bool location_of_click_is_ground()
    {
        // null exception when no scene object clicked?
        return _hit.transform.CompareTag("Ground");
    }

    private void get_location_of_click()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out _hit);
    }

    private bool camera_is_not_null()
    {
        return Camera.main != null;
    }

    private bool left_mouse_button_is_down()
    {
        return Input.GetMouseButtonDown(0);
    }
}



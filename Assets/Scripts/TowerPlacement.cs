using System;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

// Author: Dante Borden
//
// Created: 4.27.2025
//
// Description:
//
// Handles player-controlled tower placement in the game world.
// Casts a ray from the mouse position to detect valid ground areas for tower spawning.
//
// Modified:
//   4.27.25
//   fixed typos, unresolved references, corrected code

public class TowerPlacement : MonoBehaviour
{
    public Vector3 place;
    public GameObject tower;

    private RaycastHit _hit;

    public bool placing;
    private Ray ray;


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
        place = _hit.point;
        Instantiate(tower, place, Quaternion.identity);
        placing = false;
    }

    private bool location_of_click_is_ground()
    {
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



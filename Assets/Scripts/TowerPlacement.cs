using System;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
        double snapToGridX = Math.Round(_hit.point.x);
        double snapToGridY = Math.Round(_hit.point.y);
        double snapToGridZ = Math.Round(_hit.point.z);
        Vector3 snapToGridplace = new Vector3((float)snapToGridX, (float)snapToGridY, (float)snapToGridZ);
        Instantiate(tower, snapToGridplace, Quaternion.identity);
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



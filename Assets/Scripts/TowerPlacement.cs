using UnityEngine;

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

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out _hit))
                {
                    if (_hit.transform.CompareTag("Ground"))
                    {
                        place = _hit.point;

                        Instantiate(tower, place, Quaternion.identity);

                        placing = false;
                    }
                }
            }
        }
    }
}


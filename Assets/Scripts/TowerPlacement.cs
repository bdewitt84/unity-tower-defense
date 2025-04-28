using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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


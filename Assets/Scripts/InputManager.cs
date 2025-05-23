// ./Assets/Scripts/InputManager.cs

using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
            }
        }
    }

    private void RequestTowerPlacement(RaycastHit hit)
    {
        Vector3 placeAt = hit.point;
        GameEvents.TowerPlacementRequest(placeAt);
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

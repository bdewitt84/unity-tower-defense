using UnityEngine;

public abstract class InputStateBase {

    protected InputManager _inputManager;

    public InputStateBase(InputManager inputManager)
    {
        _inputManager = inputManager;
    }

    public abstract void Update();
    public abstract void Enter();
    public abstract void Exit();

    protected bool HitIsGround(RaycastHit hit)
    {
        return hit.transform != null && hit.transform.CompareTag("Ground");
    }

    protected bool GetRaycastHit(out RaycastHit hit, LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);   
        return Physics.Raycast(ray, out hit, 100f, layer);
    }

    protected bool CameraIsNotNull()
    {
        return Camera.main != null;
    }

    protected bool LeftMouseButtonIsDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    protected bool RightMouseButtonIsDown()
    {
        return Input.GetMouseButtonDown(1);
    }

}

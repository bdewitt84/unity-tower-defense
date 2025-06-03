using UnityEngine;

public class InputStateBuild : InputStateBase
{
    private GameObject _towerPrefab;
    private bool _disableRequested = false;

    public InputStateBuild(InputManager inputManager, GameObject towerPrefab) : base(inputManager)
    {
        _towerPrefab = towerPrefab;
    }

    public override void Enter()
    {
        GameEvents.OnTowerSelected += HandleTowerSelected;
    }

    public override void Exit()
    {
        GameEvents.OnTowerSelected -= HandleTowerSelected;
    }

    public override void Update()
    {
        if (LeftMouseButtonIsDown())
        {
            RaycastHit hit;
            if (GetRaycastHit(out hit, _inputManager.GroundLayer | _inputManager.PathLayer))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    Debug.Log($"Build click at ({hit.point})");
                    GameEvents.TowerPlacementRequest(hit.point, _towerPrefab);
                }
            }
            
        }
        if (RightMouseButtonIsDown())
        {
            Debug.Log("Deselected");
            _disableRequested = true;
            GameEvents.TowerPreviewDisable();
            _inputManager.SetState(new InputStateDefault(_inputManager));
        }
        else
        {
            RaycastHit hit;
            if (GetRaycastHit(out hit, _inputManager.GroundLayer | _inputManager.PathLayer))
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    _disableRequested = false;
                    GameEvents.TowerPreviewRequest(hit.point, _towerPrefab);
                }
            }
            else if (!_disableRequested)
            {
                _disableRequested = true;
                GameEvents.TowerPreviewDisable();
            }
        }
    }

    void HandleTowerSelected(GameObject tower)
    {
        _towerPrefab = tower;
    }
}

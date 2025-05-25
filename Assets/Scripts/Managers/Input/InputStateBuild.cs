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
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        if (LeftMouseButtonIsDown())
        {
            GetRaycastHit(out RaycastHit hit, _inputManager.GroundLayer);
            Debug.Log($"Build click at ({hit.point})");
            GameEvents.TowerPlacementRequest(hit.point, _towerPrefab);
        }
        if (RightMouseButtonIsDown())
        {
            Debug.Log("Deselected");
            _inputManager.SetState(new InputStateDefault(_inputManager));
        }
        else
        {
            if (GetRaycastHit(out RaycastHit hit, _inputManager.GroundLayer))
            {
                _disableRequested = false;
                Debug.Log($"InputStateBuild: Tower prefab being sent to preview: {_towerPrefab?.name}");
                GameEvents.TowerPreviewRequest(hit.point, _towerPrefab);
            }
            else if (!_disableRequested)
            {
                _disableRequested = true;
                GameEvents.TowerPreviewDisable();
            }
        }
    }
}

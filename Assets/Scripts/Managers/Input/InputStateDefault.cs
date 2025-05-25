using UnityEngine;

public class InputStateDefault : InputStateBase
{
    public InputStateDefault(InputManager inputManager) : base(inputManager)
    {
    }

    public override void Enter()
    {
        GameEvents.OnTowerSelected += HandleTowerSelected;
    }

    public override void Exit()
    {
        GameEvents.OnTowerSelected -= HandleTowerSelected;
    }

    private void HandleTowerSelected(GameObject towerPrefab)
    {
        _inputManager.SetState(new InputStateBuild(_inputManager, towerPrefab));
    }

    public override void Update()
    {
        if (LeftMouseButtonIsDown())
        {
            // if we clicked on an instance of a tower
            // select the instance of the tower
            Debug.Log("Left mouse down");
        }
    }
}


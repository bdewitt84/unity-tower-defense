// ./assets/scripts/managers/input/inputManager.cs


using UnityEngine;


// Author: Brett DeWitt
//
// Date: 5.23.2025
//
// Handles inputs using a finite state machine implementation


public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _pathLayer;

    public LayerMask GroundLayer => _groundLayer;
    public LayerMask PathLayer => _pathLayer;

    private InputStateBase _currentState;

    protected InputStateBase _defaultState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeStates();
        SetState(_defaultState);
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.Update();
    }

    private void InitializeStates()
    {
        _defaultState = new InputStateDefault(this);
    }

    public void SetState(InputStateBase state)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }
        _currentState = state;
        _currentState.Enter();
    }
}

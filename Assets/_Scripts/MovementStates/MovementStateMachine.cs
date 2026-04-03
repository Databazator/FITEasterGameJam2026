using System;
using System.Collections.Generic;
using System.Linq;

public class MovementStateMachine
{
    private MovementState _currentState;
    public MovementState CurrentState => _currentState;

    public void SetupStates(PlayerController core)
    {
        MovementState[] states = core.GetComponentsInChildren<MovementState>();
        foreach (var state in states)
        {
            state.Setup(core);
        }
    }

    public void SetState(MovementState newState, bool forceReset = false)
    {
        if (_currentState != newState || forceReset)
        {
            MovementState prevState = _currentState;

            _currentState?.Exit();
            _currentState = newState;
            _currentState.Initialise();
            _currentState.Enter();

            //Debug.LogWarning(prevState + " -> " + _currentState);
        }
    }

}

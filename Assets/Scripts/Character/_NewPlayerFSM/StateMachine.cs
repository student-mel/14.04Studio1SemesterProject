using System;
using System.Collections.Generic;

public class StateMachine
{
    private Dictionary<Type, IState> states = new Dictionary<Type, IState>();
    public IState CurrentState { get; private set; }

    public void AddState(IState state)
    {
        states[state.GetType()] = state;
    }

    public void ChangeState<T>() where T : IState
    {
        if (CurrentState != null)
            CurrentState.Exit();

        CurrentState = states[typeof(T)];
        CurrentState.Enter();
    }

    public void Update() => CurrentState?.Update();
    public void FixedUpdate() => CurrentState?.FixedUpdate();
}


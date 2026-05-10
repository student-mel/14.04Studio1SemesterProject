using UnityEngine;

namespace Character.PlayerHFSM
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentState {get; set;}
        
        public void Initialise(PlayerState state)
        {
            CurrentState = state;
            state.EnterState();
        }

        public void ChangeState(PlayerState newState)
        {
            if (CurrentState == newState) return;
            CurrentState?.ExitState();
            CurrentState = newState;
                CurrentState.EnterState();
        }
    }
}

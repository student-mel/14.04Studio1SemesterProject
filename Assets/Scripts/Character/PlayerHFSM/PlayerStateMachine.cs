using UnityEngine;

namespace Character.PlayerHFSM
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentSubState { get; set; }
        public PlayerState PreviousSubState { get; set; }
        public PlayerState CurrentState {get; private set;}
        
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

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
            CurrentState?.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }

        public void TryAttack(StateAttack newState, CharacterMove move)
        {
            Debug.LogWarning("TryAttack");
            CurrentState?.ExitState();
            CurrentState = newState;
            newState.move = move;
            CurrentState.EnterState();
        }
    }
}

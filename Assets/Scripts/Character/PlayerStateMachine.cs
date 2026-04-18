using UnityEngine;

namespace Player
{
    public class PlayerStateMachine
    {
        public PlayerState CurrentState {get; set;}
        private PlayerState currentSubState;
        private PlayerState defaultSubState;
        private PlayerState parent;
        
        private Dictionary<Type, StateMachine> subStates = new Dictionary<Type, StateMachine>();

        public void Initialise(PlayerState state)
        {
            CurrentState = state;
            state.EnterState();
        }

        public void ChangeState(PlayerState newState)
        {
            CurrentState.ExitState();
            CurrentState = newState;
            CurrentState.EnterState();
        }
    }
}

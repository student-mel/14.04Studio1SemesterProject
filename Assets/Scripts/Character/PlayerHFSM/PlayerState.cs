using UnityEngine;

namespace Character.PlayerHFSM
{
    public class PlayerState
    {
        protected PlayerController Player;
        protected PlayerStateMachine StateMachine;
        
        private PlayerState _currentSubState;

        public PlayerState(PlayerController player, PlayerStateMachine stateMachine)
        {
            this.Player = player;
            this.StateMachine = stateMachine;
        }

        public virtual void EnterState()
        {
            Debug.Log($"Entering {ToString()}");
        }

        public virtual void ExitState()
        {
            _currentSubState?.ExitState();
        }

        public virtual void UpdateState()
        {
            _currentSubState?.UpdateState();
        }

        public virtual void FixedUpdateState()
        {
            _currentSubState?.FixedUpdateState();
        }

        public virtual void LateUpdateState()
        {
            _currentSubState?.LateUpdateState();
        }

        public virtual void AnimationTriggerState(PlayerController.AnimationTriggerType trigger)
        {
            _currentSubState?.AnimationTriggerState(trigger);
        }
        
        /*protected void SetSubState(PlayerState newSubState)
        {
            if (_currentSubState == newSubState) return;
            _currentSubState = newSubState;
            _currentSubState.EnterState();
        }*/

        protected void ChangeSubState(PlayerState newSubState)
        {
            if (_currentSubState == newSubState) return;
            
            _currentSubState?.ExitState();
            _currentSubState = newSubState;
            _currentSubState.EnterState();
        }

        protected PlayerState GetSubState()
        {
            return _currentSubState;
        }
    }
}

using UnityEngine;

namespace Player
{
    public class PlayerState
    {
        protected PlayerController player;
        protected PlayerStateMachine stateMachine;
        
        private PlayerState currentSubState;

        public PlayerState(PlayerController player, PlayerStateMachine stateMachine)
        {
            this.player = player;
            this.stateMachine = stateMachine;
        }
        
        public virtual void EnterState() {}

        public virtual void ExitState()
        {
            currentSubState?.ExitState();
        }

        public virtual void UpdateState()
        {
            currentSubState?.UpdateState();
        }

        public virtual void FixedUpdateState()
        {
            currentSubState?.FixedUpdateState();
        }

        public virtual void LateUpdateState()
        {
            currentSubState?.LateUpdateState();
        }

        public virtual void AnimationTriggerState(PlayerController.AnimationTriggerType trigger)
        {
            currentSubState?.AnimationTriggerState(trigger);
        }
        
        protected void SetSubState(PlayerState newSubState)
        {
            currentSubState = newSubState;
            currentSubState.EnterState();
        }

        protected void ChangeSubState(PlayerState newSubState)
        {
            currentSubState?.ExitState();
            currentSubState = newSubState;
            currentSubState.EnterState();
        }
    }
}

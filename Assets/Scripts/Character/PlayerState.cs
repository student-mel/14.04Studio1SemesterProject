using UnityEngine;

namespace Player
{
    public class PlayerState
    {
        protected PlayerController player;
        protected PlayerStateMachine stateMachine;

        public PlayerState(PlayerController player, PlayerStateMachine stateMachine)
        {
            this.player = player;
            this.stateMachine = stateMachine;
        }
        
        public virtual void EnterState() {}
        public virtual void ExitState() {}
        public virtual void UpdateState() {}
        public virtual void FixedUpdateState() {}
        public virtual void LateUpdateState() {}
        public virtual void AnimationTriggerState(PlayerController.AnimationTriggerType trigger) {}
    }
}

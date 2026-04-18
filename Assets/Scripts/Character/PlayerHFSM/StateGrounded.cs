using Character;
using Character.PlayerHFSM;

public class StateGrounded : PlayerState
{
    private PlayerState IdleState, MoveState, CrouchState;
    
    public StateGrounded(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        IdleState = new SubStateIdle(player, stateMachine);
        MoveState = new SubStateMove(player, stateMachine);
        CrouchState = new SubStateCrouch(player, stateMachine);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void AnimationTriggerState(PlayerController.AnimationTriggerType trigger)
    {
        base.AnimationTriggerState(trigger);
    }
}

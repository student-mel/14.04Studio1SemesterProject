using Character;
using Character.PlayerHFSM;
using RPGCharacterAnims.Actions;

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
        Player.canFlip = true;
        ChangeSubState(IdleState);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Player.MoveName.StartsWith("Jump")) Jump();
        else if (Player.MoveName.StartsWith("Move")) Move();
        else if (Player.MoveName.StartsWith("Crouch")) Crouch();
        else if (Player.MoveName.StartsWith("Null")) Idle();
    }

    void Idle()
    {
        ChangeSubState(IdleState);
    }

    void Jump()
    {
        StateMachine.ChangeState(Player.AirborneState);
    }

    void Move()
    {
        ChangeSubState(MoveState);
    }

    void Crouch()
    {
        ChangeSubState(CrouchState);
    }
}

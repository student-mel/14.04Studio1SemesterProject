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
        SetSubState(IdleState);
    }

    public override void UpdateState()
    {
        base.UpdateState();
        if (Player.MoveInput.y > 0)
            Jump();
        else if (Player.MoveInput.y == 0 &&  Player.MoveInput.x != 0)
            Move();
        else
            Crouch();
    }

    void Jump()
    {
        StateMachine.ChangeState(Player.AirborneState);
    }

    void Move()
    {
        SetSubState(MoveState);
    }

    void Crouch()
    {
        SetSubState(CrouchState);
    }
}

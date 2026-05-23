using Character;
using Character.PlayerHFSM;
using RPGCharacterAnims.Actions;
using UnityEngine;
using UnityEngine.Profiling;

public class StateGrounded : PlayerState
{
    private PlayerState IdleState, MoveState, CrouchState;
    
    bool jumpConsumed = false;
    
    public StateGrounded(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        IdleState = new SubStateIdle(player, stateMachine);
        MoveState = new SubStateMove(player, stateMachine);
        CrouchState = new SubStateCrouch(player, stateMachine);
        canAttack = true;
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.CanFlip = true;
        ChangeSubState(IdleState);
        //jumped = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        bool jumpHeld = Player.MoveDir.y > 0;
        if (jumpHeld && !jumpConsumed) Jump();
        else if (Player.MoveDir.y < 0) Crouch();
        else if (Mathf.Abs(Player.MoveDir.x) > 0) Move();
        else if (Player.MoveDir == Vector2.zero) Idle();
        if (!jumpHeld) jumpConsumed = false;
        return;

        //Debug.LogWarning(Player.AttackName);
//        Debug.LogWarning(Player.ReactionName);
        /*if (!Player.ReactionName.StartsWith("Null")) TryStun();
        else if (!Player.AttackName.StartsWith("Null")) TryAttack();
        else if (Player.MoveName.StartsWith("Jump")) Jump();
        else if (Player.MoveName.StartsWith("Move")) Move();
        else if (Player.MoveName.StartsWith("Crouch")) Crouch();
        else if (Player.MoveName.StartsWith("Null")) Idle();*/
    }

    private void TryStun()
    {
        StateMachine.ChangeState(Player.StunState);
    }

    void Idle()
    {
        ChangeSubState(IdleState);
    }

    void Jump()
    {
        jumpConsumed = true;
        
        // tutorial emits
        EventBus.Emit("p1_jump", Player.player);
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

    void TryAttack()
    {
        //Debug.LogWarning("TryAttack");
        StateMachine.ChangeState(Player.AttackState);
    }
}

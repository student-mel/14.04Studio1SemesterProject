using Character;
using Character.PlayerHFSM;
using RPGCharacterAnims.Actions;
using UnityEngine;

public class StateGrounded : PlayerState
{
    private PlayerState IdleState, MoveState, CrouchState;
    
    public StateGrounded(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        IdleState = new SubStateIdle(player, stateMachine);
        MoveState = new SubStateMove(player, stateMachine);
        CrouchState = new SubStateCrouch(player, stateMachine);
        string p = Player.player ==  PlayerController.PlayerEnum.PlayerOne ? "p1_" : "p2_";
        EventBus.Subscribe($"{p}attack", OnAttack);
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.canFlip = true;
        ChangeSubState(IdleState);
        
    }

    public override void ExitState()
    {
        base.ExitState();
        string p = Player.player ==  PlayerController.PlayerEnum.PlayerOne ? "p1_" : "p2_";
        //EventBus.Unsubscribe($"{p}attack", OnAttack);
    }

    private void OnAttack(object obj)
    {
        TryAttack(obj as CharacterMove);
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

    void TryAttack(CharacterMove move)
    {
        StateMachine.TryAttack((StateAttack)Player.AttackState, move);
    }
}

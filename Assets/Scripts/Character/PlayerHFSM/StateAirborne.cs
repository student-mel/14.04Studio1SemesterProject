using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class StateAirborne : PlayerState
{
    private PlayerState RiseState, FallState;
    bool isFalling;
    
    CapsuleCollider playerCollider;
    CapsuleCollider enemyCollider;
    
    public StateAirborne(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        RiseState = new SubStateRise(player, stateMachine);
        FallState = new SubStateFall(player, stateMachine);
        playerCollider = player.RB.GetComponent<CapsuleCollider>();
        enemyCollider = player.opponent.RB.GetComponent<CapsuleCollider>();
        canAttack = true;
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.canFlip = false;
        ChangeSubState(RiseState);
        isFalling = false;
        
        Physics.IgnoreCollision(playerCollider, enemyCollider, true);
    }

    public override void ExitState()
    {
        base.ExitState();
        isFalling = false;
        Physics.IgnoreCollision(playerCollider, enemyCollider, false);
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
        //Debug.Log(Player.RB.linearVelocity.y);
        if (Player.RB.linearVelocity.y < 0 && !isFalling)
        {
            isFalling = true;
            ChangeSubState(FallState);
        }
    }
}

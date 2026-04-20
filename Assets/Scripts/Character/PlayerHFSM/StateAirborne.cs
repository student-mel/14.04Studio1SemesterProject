using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class StateAirborne : PlayerState
{
    private PlayerState RiseState, FallState;
    bool isFalling;
    
    public StateAirborne(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        RiseState = new SubStateRise(player, stateMachine);
        FallState = new SubStateFall(player, stateMachine);
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.canFlip = false;
        ChangeSubState(RiseState);
        isFalling = false;
        Player.RB.GetComponent<CapsuleCollider>().enabled = false;
        //Player.RB.GetComponent<CapsuleCollider>().height *= 0.5f;
    }

    public override void ExitState()
    {
        base.ExitState();
        isFalling = false;
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

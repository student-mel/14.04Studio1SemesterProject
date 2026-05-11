using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class SubStateIdle : PlayerState
{
    public SubStateIdle(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        canAttack = true;
        StateMachine.PreviousSubState = this;
    }

    public override void EnterState()
    {
        base.EnterState();
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
}

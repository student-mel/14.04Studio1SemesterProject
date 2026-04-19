using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class SubStateRise : PlayerState
{
    private static readonly int Jump = Animator.StringToHash("jump");

    public SubStateRise(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        Player.RB.AddForce(Vector3.up * Player.JumpForce, ForceMode.Impulse);
        Player.animator.SetTrigger(Jump);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }
}

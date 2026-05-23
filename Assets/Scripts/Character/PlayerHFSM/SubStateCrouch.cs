using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class SubStateCrouch : PlayerState
{
    private static readonly int Crouching = Animator.StringToHash("crouching");
    private static readonly int Block = Animator.StringToHash("block");

    public SubStateCrouch(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        canAttack = true;
    }

    public override void EnterState()
    {
        base.EnterState();

        EventBus.Emit("tutorial_crouch", Player.player);
        AnimateCrouching(true);
    }

    public override void ExitState()
    {
        base.ExitState();
        AnimateCrouching(false);
    }

    public override void UpdateState()
    {
        base.UpdateState();
    }
    
    private void AnimateCrouching(bool isCrouching)
    {
        Player.animator.SetBool(Crouching, isCrouching);
    }

    void AnimateBlock()
    {
        Player.animator.SetTrigger(Block);
    }
}

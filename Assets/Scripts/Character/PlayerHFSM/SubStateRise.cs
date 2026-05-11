using System.Collections;
using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class SubStateRise : PlayerState
{
    private static readonly int Jump = Animator.StringToHash("jump");

    public SubStateRise(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
        canAttack = true;
    }

    public override void EnterState()
    {
        base.EnterState();
        AddJumpForce();
        Player.animator.SetTrigger(Jump);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    void AddJumpForce()
    {
        Vector3 force = Vector3.up;
        AudioManager.Instance?.PlayJump(Player.gameObject);
        if (Player.MoveDir.x == 0)
            Player.RB.AddForce(force * Player.JumpForce, ForceMode.Impulse);
        else if (Player.MoveDir.x < 0)
        {
            force.x = -0.5f;
            Player.RB.AddForce(force * Player.JumpForce, ForceMode.Impulse);
        }
        else
        {
            force.x = 0.5f;
            Player.RB.AddForce(force * Player.JumpForce, ForceMode.Impulse);
        }

    }
   
}

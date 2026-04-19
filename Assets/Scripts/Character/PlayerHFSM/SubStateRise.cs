using System.Collections;
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
        AddJumpForce();
        Player.animator.SetTrigger(Jump);
    }

    void AddJumpForce()
    {
        Vector3 force = Vector3.up;
        
        if (Player.MoveName.EndsWith("Up"))
            Player.RB.AddForce(force * Player.JumpForce, ForceMode.Impulse);
        else if (Player.MoveName.EndsWith("Left"))
        {
            force.x = -0.5f;
            Player.RB.AddForce(force * Player.JumpForce, ForceMode.Impulse);
        }
        else if (Player.MoveName.EndsWith("Right"))
        {
            force.x = 0.5f;
            Player.RB.AddForce(force * Player.JumpForce, ForceMode.Impulse);
        }

    }
   
}

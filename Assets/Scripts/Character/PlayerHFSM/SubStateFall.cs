using System.Collections;
using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class SubStateFall: PlayerState
{
    private static readonly int Fall = Animator.StringToHash("fall");

    public SubStateFall(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        hasFallen = false;
        Player.RB.AddForce(-Vector3.up * Player.JumpForce, ForceMode.Impulse);
    }

    public override void ExitState()
    {
        base.ExitState();
        hasFallen = false;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (Physics.Raycast(Player.transform.position, Vector3.down, out var hit,0.15f, LayerMask.GetMask("Ground")))
        {
            HitGround(hit);
        }
    }

    bool hasFallen;
    void HitGround(RaycastHit hit)
    {
        if (hasFallen) return;
        hasFallen = true;
        Player.animator.SetTrigger(Fall);
        StateMachine.ChangeState(Player.GroundedState);
        Player.RB.position = new Vector3(
            Player.RB.position.x,
            hit.transform.position.y + 0.1f,
            Player.RB.position.z);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Player.transform.position, Player.transform.position + Vector3.down * 0.1f);
    }
}

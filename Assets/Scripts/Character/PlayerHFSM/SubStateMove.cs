using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class SubStateMove : PlayerState
{
    private static readonly int MoveForward = Animator.StringToHash("moveForward");
    private static readonly int MoveBackward = Animator.StringToHash("moveBackward");
    
    bool isMovingRight = false;

    public SubStateMove(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        AudioManager.Instance?.PlayWalk();
    }

    public override void ExitState()
    {
        base.ExitState();
        Player.animator.SetBool(MoveForward, false);
        Player.animator.SetBool(MoveBackward, false);
        Player.RB.linearVelocity = Vector3.zero;
    }

    public override void UpdateState()
    {
        base.UpdateState();
        bool isMoving = Player.MoveName.EndsWith("Left") || Player.MoveName.EndsWith("Right");

        if (isMoving)
        {
            AudioManager.Instance?.PlayWalk();

            if (Player.MoveName.EndsWith("Left"))
                AnimateMove(false);
            else if (Player.MoveName.EndsWith("Right"))
                AnimateMove(true);
        }
        else
        {
            AudioManager.Instance?.StopWalk();
        }
    }

    void Move()
    {
        Debug.Log("Move");
        Vector3 displacement = Player.RelativeDir * Player.MoveSpeed * (isMovingRight?1:-1) * Time.fixedDeltaTime;
    
        Player.RB.MovePosition(Player.RB.position + displacement);
    }

    private void AnimateMove(bool isRight)
    {
        isMovingRight = isRight;
        
        Player.animator.SetBool(MoveForward, isRight == Player.IsFacingRight);
        Player.animator.SetBool(MoveBackward, isRight ^ Player.IsFacingRight);
        
        Player.RB.linearVelocity = Vector3.right * Player.MoveSpeed * (isMovingRight?1:-1);
    }

}

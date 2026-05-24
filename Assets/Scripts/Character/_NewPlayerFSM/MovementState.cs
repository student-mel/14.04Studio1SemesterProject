using UnityEngine;

public class MovementState : BaseState, IState
{
    private static readonly int MoveForward = Animator.StringToHash("moveForward");
    private static readonly int MoveBackward = Animator.StringToHash("moveBackward");
    private static readonly int JumpTrigger = Animator.StringToHash("jump");
    private static readonly int Crouching = Animator.StringToHash("crouching");
    private bool jumpConsumed = false;
    private bool collisionIgnored = false;

    public MovementState(PlayerBehaviour pb, StateMachine fsm) : base(pb, fsm)
    {
    }

    public void Enter()
    {
        pb.CanFlip = true;
        jumpConsumed = false;
    }

    public void Update()
    {
        if (pb.RB.linearVelocity.y <= 0)
            jumpConsumed = false;
        
        if (pb.IsGrounded)
        {
            if (pb.MoveDir.y > 0 && !jumpConsumed)
            {
                Jump();
                jumpConsumed = true;
                return;
            }

            Crouch();
        }
    }

    private void Crouch()
    {
        /*pb.animator.SetBool(MoveForward, !isCrouching);
        pb.animator.SetBool(MoveBackward, !isCrouching);*/
        
        pb.CanFlip = !pb.IsCrouching;
        pb.animator.SetBool(Crouching, pb.IsCrouching);
    }

    public void FixedUpdate()
    {
        if (!pb.IsGrounded)
            return;
        
        if (jumpConsumed)
            return;
        
        if (collisionIgnored && pb.RB.linearVelocity.y <= 0)
        {
            pb.animator.SetBool(JumpTrigger, false);
            Physics.IgnoreCollision(pb.playerColl, pb.opponent.playerColl, false);
            collisionIgnored = false;
            pb.CanFlip = true;
        }
        
        if (pb.IsCrouching)
            return;
        
        float x = pb.MoveDir.x;
        SetWalkAnimation();
        
        if (Mathf.Abs(x) < 0.1f)
            return;
        
        pb.RB.linearVelocity = new Vector3(x * pb.MoveSpeed, pb.RB.linearVelocity.y, 0);
    }

    public void Exit()
    {
        pb.animator.SetBool(MoveForward, false);
        pb.animator.SetBool(MoveBackward, false);
    }

    private void SetWalkAnimation()
    {
        float x = pb.MoveDir.x;

        if (Mathf.Abs(x) < 0.1f)
        {
            pb.animator.SetBool(MoveForward, false);
            pb.animator.SetBool(MoveBackward, false);
            return;
        }

        bool isMovingRight = x > 0;

        pb.animator.SetBool(MoveForward, isMovingRight == pb.IsFacingRight);
        pb.animator.SetBool(MoveBackward, isMovingRight ^ pb.IsFacingRight);
    }

    public void Jump()
    {
        pb.CanFlip = false;

        // tutorial emits
        EventBus.Emit("p1_jump", pb.player);
        AudioManager.Instance?.PlayJump(pb.gameObject);
        pb.animator.SetBool(JumpTrigger, true);

        Vector3 force = Vector3.up;
        force.x = pb.MoveDir.x < 0 ? -0.25f : 0.25f;
        force.x = pb.MoveDir.x == 0 ? 0 : force.x;

        pb.RB.linearVelocity = force * pb.JumpForce;

        Physics.IgnoreCollision(pb.playerColl, pb.opponent.playerColl, true);
        collisionIgnored = true;
    }
}
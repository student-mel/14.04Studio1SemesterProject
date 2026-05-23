using UnityEngine;

public interface IMoveable
{
    Rigidbody RB { get; }
    bool IsFacingRight { get; set; }
    bool CanFlip { get; set; }
    bool IsGrounded { get; }
    
    Vector3 RelativeDir { get; }
    Vector2 MoveDir { get; }
    void CheckRelativeDir();
    
    float MoveSpeed { get; }
    float JumpForce { get; }
}

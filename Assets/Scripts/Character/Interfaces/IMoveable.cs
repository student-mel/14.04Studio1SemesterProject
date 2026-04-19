using UnityEngine;

public interface IMoveable
{
    Rigidbody RB { get; }
    bool IsFacingRight { get; set; }

    Vector3 RelativeDir { get; }
    //Vector2 MoveInput { get; }
    void CheckRelativeDir();
    
    float MoveSpeed { get; }
    float JumpForce { get; }
}

using UnityEngine;

public interface IMoveable
{
    Rigidbody RB { get; }
    bool IsFacingRight { get; set; }

    Vector2 MoveInput { get; }
    void CheckRelativeDir();
}

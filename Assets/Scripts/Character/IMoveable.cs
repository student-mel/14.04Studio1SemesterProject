using UnityEngine;

public interface IMoveable
{
    Rigidbody RB { get; set; }
    bool IsFacingRight { get; set; }
    void OnMove(object obj);
    void CheckRelativeDir();
}

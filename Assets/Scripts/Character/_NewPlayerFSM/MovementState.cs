using UnityEngine;

public class MovementState : BaseState, IState
{
    public MovementState(PlayerBehaviour pb, StateMachine fsm) : base(pb, fsm)
    {
    }

    public void Enter()
    {
        pb.CanFlip = true;
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
        
    }

    public void Exit()
    {
    }
}

using UnityEngine;

public class AttackState : BaseState, IState
{
    public AttackState(PlayerBehaviour pb, StateMachine fsm) : base(pb, fsm)
    {
    }

    void Start()
    {
        
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
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

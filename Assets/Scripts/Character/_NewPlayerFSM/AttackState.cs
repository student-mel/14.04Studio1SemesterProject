using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState, IState
{
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Action = Animator.StringToHash("action");

    public AttackState(PlayerBehaviour pb, StateMachine fsm) : base(pb, fsm)
    {
    }
    
    public readonly Dictionary<string, int> AttackType = new()
    {
        { "Light Attack", 1 },
        { "Medium Attack", 2 },
        { "Heavy Attack", 3 }
    };

    public void Enter(object data = null)
    {
        Moveset moveset = data as Moveset;
        if (moveset != null)
        {
            pb.animator.SetFloat(Attack, AttackType[moveset.Name]);
            pb.animator.SetTrigger(Action);
        }
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

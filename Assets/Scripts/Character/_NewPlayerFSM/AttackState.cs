using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState, IState
{
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Action = Animator.StringToHash("action");

    public AttackState(PlayerBehaviour pb, StateMachine fsm) : base(pb, fsm)
    {
    }

    private readonly Dictionary<string, int> _attackType = new()
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
            int attType = _attackType[moveset.Name];
            pb.animator.SetFloat(Attack, attType);
            pb.animator.SetTrigger(Action);
            AudioManager.Instance?.PlayAttack(attType, pb.gameObject);
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
        pb.animator.SetFloat(Attack, 0);
    }
}

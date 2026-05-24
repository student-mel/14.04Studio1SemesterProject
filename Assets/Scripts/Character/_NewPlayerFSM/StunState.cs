using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class StunState : BaseState, IState
{
    private static readonly int Reaction = Animator.StringToHash("reaction");
    private static readonly int Hurt = Animator.StringToHash("hurt");

    public StunState(PlayerBehaviour pb, StateMachine fsm) : base(pb, fsm)
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
        Moveset move =  (Moveset)data;
        SetAnimation(move);
    }
    
    void SetAnimation(Moveset move)
    {
        Animator a =  pb.animator;
        int attType = _attackType[move.Name];
        
        float f = 20 + (attType - 1) * 10;
        pb.TakeDamage(move.damage);
        a.SetBool(Hurt, true);
        a.SetFloat(Reaction, (int)attType);
        
        if (pb.IsBlocking)
            f *= 1.5f;
        ApplyKnockback(f);
    }

    void StopHurting()
    {
        pb.animator.SetBool(Hurt, false);
    }
    
    void ApplyKnockback(float force)
    {
        pb.RB.AddForce(-pb.RelativeDir * force, ForceMode.Impulse);
    }

    public void Update()
    {
    }

    public void FixedUpdate()
    {
    }

    public void Exit()
    {
        pb.animator.SetBool(Hurt, false);
        pb.animator.SetFloat(Reaction, 0);
    }
}

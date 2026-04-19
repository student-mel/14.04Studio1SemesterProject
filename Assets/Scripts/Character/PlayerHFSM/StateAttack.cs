using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class StateAttack : PlayerState
{
    private static readonly int Action = Animator.StringToHash("action");
    private static readonly int Attack = Animator.StringToHash("attack");
    public StateAttack(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        SetAnimation();
        Player.animator.SetTrigger(Action);
    }

    void SetAnimation()
    {
        Animator a =  Player.animator;
        int i = 0;
        switch (Player.AttackName)
        {
            case "Light Attack":
                i = 0;
                break;
            case "Medium Attack":
                i = 1;
                break;
            case "Heavy Attack":
                i = 2;
                break;
        }
        
        a.SetFloat(Attack, (int)i);

        Player.nextAttack = "Null";
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Player.nextAttack = "Null";
        if (!Player.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Attack"))
            StateMachine.ChangeState(Player.GroundedState);
        //Debug.Log(Player.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
    }
}

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
        EventBus.Emit("start_action", (int)Player.player);
    }

    void SetAnimation()
    {
        Animator a =  Player.animator;
        string tempHitTrigger = "";
        int i = 0;
        switch (Player.AttackName)
        {
            case "Light Attack":
                //Debug.LogWarning("Light Attack");
                i = 0;
                tempHitTrigger = "Light";
                break;
            case "Medium Attack":
               // Debug.LogWarning("Medium Attack");
                
                i = 1;
                tempHitTrigger = "Medium";
                break;
            case "Heavy Attack":
               // Debug.LogWarning("Heavy Attack");
                
                i = 2;
                tempHitTrigger = "Heavy";
                break;
        }
        
        a.SetFloat(Attack, (int)i);
        Player.hitboxDebugAnimator.SetTrigger(tempHitTrigger);

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
        {
            StateMachine.ChangeState(Player.GroundedState);
            EventBus.Emit("attack_finished", Player.player);
        }
        
        //Debug.Log(Player.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
    }
}

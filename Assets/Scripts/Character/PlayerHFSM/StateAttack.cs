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
                switch (StateMachine.PreviousSubState.ToString())
                {
                    case "SubStateCrouch":
                        i = 3;
                        break;
                    case "SubStateRise":
                        i = 6;
                        break;
                    case "StateAirborne":
                        goto case "SubStateRise";
                    default:
                        i = 0;
                        break;
                }
                tempHitTrigger = "Light";
                AudioManager.Instance?.PlayLightAttack(Player.gameObject);
                break;
            case "Medium Attack":
               // Debug.LogWarning("Medium Attack");
               switch (StateMachine.PreviousSubState.ToString())
               {
                   case "SubStateCrouch":
                       i = 4;
                       break;
                   case "SubStateRise":
                       i = 7;
                       break;
                   case "StateAirborne":
                       goto case "SubStateRise";
                   default:
                       i = 1;
                       break;
               }
                tempHitTrigger = "Medium";
                AudioManager.Instance?.PlayMediumAttack(Player.gameObject);
                break;
            case "Heavy Attack":
               // Debug.LogWarning("Heavy Attack");
               switch (StateMachine.PreviousSubState.ToString())
               {
                   case "SubStateCrouch":
                       i = 5;
                       break;
                   case "SubStateRise":
                       i = 8;
                       break;
                   case "StateAirborne":
                       goto case "SubStateRise";
                   default:
                       i = 2;
                       break;
               }
                tempHitTrigger = "Heavy";
                AudioManager.Instance?.PlayHeavyAttack(Player.gameObject);
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

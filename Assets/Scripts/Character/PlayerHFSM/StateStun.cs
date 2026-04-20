using Character;
using Character.PlayerHFSM;
using UnityEngine;

public class StateStun : PlayerState
{
    private static readonly int Hurt = Animator.StringToHash("hurt");
    private static readonly int Reaction = Animator.StringToHash("reaction");
    public StateStun(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }
    
    public override void EnterState()
    {
        base.EnterState();
        SetAnimation();
        Player.animator.SetTrigger(Hurt);
    }

    void ApplyKnockback(float force)
    {
        Player.RB.AddForce(-Player.RelativeDir * force, ForceMode.Impulse);
    }

    void SetAnimation()
    {
        Animator a =  Player.animator;
        int i = 0;
        float f = 20;
        switch (Player.ReactionName)
        {
            case "Light":
                i = 0;
                Player.TakeDamage(5);
                break;
            case "Medium":
                i = 1;
                f += 10;
                Player.TakeDamage(7);
                
                break;
            case  "Heavy":
                i = 2;
                f += 20;
                Player.TakeDamage(10);
                
                break;
        }
        
        a.SetFloat(Reaction, (int)i);
        Player.nextReaction = "Null";
        ApplyKnockback(f);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        Player.nextReaction = "Null";
        if (!Player.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Reaction"))
            StateMachine.ChangeState(Player.GroundedState);
        //Debug.Log(Player.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
    }
}

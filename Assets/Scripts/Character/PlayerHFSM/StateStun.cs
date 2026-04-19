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

    void SetAnimation()
    {
        Animator a =  Player.animator;
        int i = 0;
        switch (Player.ReactionName)
        {
            case "Hit Body":
                i = 0;
                break;
            case "Hit Head":
                i = 1;
                break;
        }
        
        a.SetFloat(Reaction, (int)i);
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

using System;
using UnityEngine;

public class MovesetHandler : MonoBehaviour
{
    [Range(1, 2)] public int PlayerIndex;
    
    private bool canChain;
    private bool canAttack;

    private int chainCount = 0;

    private string player = "";

    private void OnEnable()
    {
        player = $"p{PlayerIndex}";
        EventBus.Subscribe($"{player}_attack", OnAttack);
        EventBus.Subscribe($"{player}_attack_end", OnAttackEnd);
        EventBus.Subscribe($"{player}_chain_start", OnChainStart);
        EventBus.Subscribe($"{player}_chain_end", OnChainEnd);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe($"{player}_attack", OnAttack);
        EventBus.Unsubscribe($"{player}_attack_end", OnAttackEnd);
        EventBus.Unsubscribe($"{player}_chain_start", OnChainStart);
        EventBus.Unsubscribe($"{player}_chain_end", OnChainEnd);
    }

    private void OnAttack(object move)
    {
        Moveset moveset = (Moveset)move;

        if (canChain && moveset.Name.Equals("Light Attack"))
        {
            canChain = false;
            canAttack = false;

            chainCount++;
        }
        if (!canAttack) return;

        canAttack = false;

    }
    
    private void OnAttackEnd(object nothing)
    {
        canAttack = true;
    }

    private void OnChainStart(object nothing)
    {
        canChain = true;
    }

    private void OnChainEnd(object nothing)
    {
        canChain = false;
    }
}

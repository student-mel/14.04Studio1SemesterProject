using System;
using UnityEngine;

public class ChainHandler : MonoBehaviour
{
    private PlayerChainMove player1;
    private PlayerChainMove player2;
    
    private void Start()
    {
    }

    public void ChainMove()
    {

    }

    private void P1Combo()
    {
    }
    private void P2Combo()
    {
    }

    private void AdvanceCombo()
    {
        
    }
}

public struct PlayerChainMove
{
    public int chainIndex;
    public Moveset moveset;
    public bool isActive;
}

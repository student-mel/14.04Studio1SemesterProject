using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CombatBoxManager : MonoBehaviour
{
    public PlayerBoxesController player1;
    public PlayerBoxesController player2;

    private int currFrame;

    private void Start()
    {
        player1.ActivateHurtboxes();
        player2.ActivateHurtboxes();
    }

    private void OnEnable()
    {
        EventBus.Subscribe("fixed_game_update", FixedGameUpdate);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe("fixed_game_update", FixedGameUpdate);
    }

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            
        }
    }

    private void FixedGameUpdate(object frame)
    {
        currFrame = (int)frame;
        ActivateHitboxesForFrame(currFrame);
        TransformBoxesToWorldSpace(currFrame);
        CheckOverlaps();
    }

    private void ActivateHitboxesForFrame(int frame)
    {
        player1.ActivateHitboxes(frame);
        player2.ActivateHitboxes(frame);
    }

    private void TransformBoxesToWorldSpace(int frame)
    {
        player1.UpdateBoxes();
        player2.UpdateBoxes();
    }

    private void CheckOverlaps()
    {
        foreach (CombatBox hitbox in player1.activeHitboxes)
        {
            foreach (CombatBox hurtbox in player2.activeHurtboxes)
            {
                if (Overlaps(hitbox.worldBox, hurtbox.worldBox))
                {
                    EventBus.Emit("p2_hurt");
                    goto Next;
                }
            }
        }
        Next:
        foreach (CombatBox hitbox in player2.activeHitboxes)
        {
            foreach (CombatBox hurtbox in player1.activeHurtboxes)
            {
                if (Overlaps(hitbox.worldBox, hurtbox.worldBox))
                {
                    EventBus.Emit("p1_hurt");
                }
            }
        }
    }

    private bool Overlaps(Box a, Box b)
    {
        return Mathf.Abs(a.center.x - b.center.x) < (a.size.x + b.size.x) * 0.5f &&
               Mathf.Abs(a.center.y - b.center.y) < (a.size.y + b.size.y) * 0.5f;
    }
}

public enum BoxType
{
    Hit,
    Hurt,
    Throw,
    Projection
}

[System.Serializable]
public class TimingWindows
{
    public int startUp;
    public int active;
    public int recover;

    public int chainStart;
    public int chainEnd;
}
[System.Serializable]
public class CombatData
{
    public CombatData(){}

    public CombatData(CombatData data)
    {
        StartFrame = data.StartFrame;
        Windows =  data.Windows;
        Positions =  data.Positions;
    }
    
    public int StartFrame;
    public TimingWindows Windows;
    [Tooltip("positions of box during active frames")]
    public Box[] Positions;

    public int StartUpStart => StartFrame + 1;
    public int StartUpEnds => StartFrame + Windows.startUp;
    public int ActiveStarts => StartUpEnds + 1;
    public int ActiveEnds => StartUpEnds + Windows.active;
    public int RecoverStarts => ActiveEnds + 1;
    public int RecoverEnds => ActiveEnds + Windows.recover;
    
    public int ChainStart => StartFrame + Windows.chainStart;
    public int ChainEnd => StartFrame + Windows.chainEnd;
}

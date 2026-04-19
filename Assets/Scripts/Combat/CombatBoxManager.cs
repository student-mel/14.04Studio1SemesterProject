using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatBoxManager : MonoBehaviour
{
    public CombatBox p1Hurtbox;
    public CombatBox p2Hurtbox;
    public CombatBox p1Hitbox;
    public CombatBox p2Hitbox;
    public CombatBox p1ThrowBox;
    public CombatBox p2ThrowBox;
    public CombatBox p1ProjectionBox;
    public CombatBox p2ProjectionBox;

    public List<CombatBox> activeHitboxes =  new List<CombatBox>();
    public List<CombatBox> activeHurtboxes =  new List<CombatBox>();

    private int currFrame;

    private void Start()
    {
        p1Hurtbox.isActive = true;
        p2Hurtbox.isActive = true;

        p1Hitbox.isActive = false;
        p2Hitbox.isActive = false;
    }

    private void OnEnable()
    {
        EventBus.Subscribe("fixed_game_update", FixedGameUpdate);
        
        activeHurtboxes.Add(p2Hurtbox);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe("fixed_game_update", FixedGameUpdate);
    }

    private void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            p1Hitbox.combatData.StartFrame = currFrame;
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
        if (p1Hitbox.combatData != null)
        {
            if (frame >= p1Hitbox.combatData.ActiveStarts && frame <= p1Hitbox.combatData.ActiveEnds)
            {
                if(!activeHitboxes.Contains(p1Hitbox))
                    activeHitboxes.Add(p1Hitbox);
                p1Hitbox.isActive = true;
            }
            else
            {
                if(activeHitboxes.Contains(p1Hitbox))
                    activeHitboxes.Remove(p1Hitbox);
                p1Hitbox.isActive = false;
            }
        }
    }

    private void TransformBoxesToWorldSpace(int frame)
    {
        p1Hitbox.ToWorld(frame);
    }

    private void CheckOverlaps()
    {
        foreach (CombatBox hitbox in activeHitboxes)
        {
            foreach (CombatBox hurtbox in activeHurtboxes)
            {
                // if (Overlaps(hitbox.worldBox, hurtbox.worldBox))
                // {
                //     Debug.Log("Player overlaps");
                // }
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

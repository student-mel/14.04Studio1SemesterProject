using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoxesController : MonoBehaviour
{
    [Range(1, 2)] public int PlayerIndex;
    
    public CombatBox[] hitboxes;
    public CombatBox[] hurtboxes;

    public List<CombatBox> activeHitboxes = new List<CombatBox>();
    public List<CombatBox> activeHurtboxes = new List<CombatBox>();

    public bool debug;

    private void Awake()
    {
        Init(ref hitboxes, BoxType.Hit);
        Init(ref hurtboxes, BoxType.Hurt);
    }
    
    private void Init(ref CombatBox[] boxes, BoxType type)
    {
        if (boxes == null && boxes.Length > 0)
        {
            for (int i = 0; i < boxes.Length; i++)
            {
                CombatBox newBox = new CombatBox(transform, boxes[i]);
                boxes[i] = newBox;
                boxes[i].type = type;
            }
        }
    }

    private void OnEnable()
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Subscribe("p1_attack", UpdateCombatData);
                break;
            case 2:
                EventBus.Subscribe("p2_attack", UpdateCombatData);
                break;
        }
    }
    private void OnDisable()
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Unsubscribe("p1_attack", UpdateCombatData);
                break;
            case 2:
                EventBus.Unsubscribe("p2_attack", UpdateCombatData);
                break;
        }
    }

    public void UpdateBoxes()
    {
        foreach (CombatBox combatBox in hitboxes)
        {
            combatBox.UpdateBox();
        }
        foreach (CombatBox combatBox in hurtboxes)
        {
            combatBox.UpdateBox();
        }
    }

    public void ActivateHurtboxes()
    {
        foreach (CombatBox hurtbox in hurtboxes)
        {
            hurtbox.isActive = true;
            if(!activeHurtboxes.Contains(hurtbox))
                activeHurtboxes.Add(hurtbox);
        }
    }

    public void ActivateHitboxes(int frame)
    {
        foreach (CombatBox hitbox in hitboxes)
        {
            if (frame >= hitbox.combatData.ActiveStarts && frame <= hitbox.combatData.ActiveEnds)
            {
                if(!activeHitboxes.Contains(hitbox))
                    activeHitboxes.Add(hitbox);
                if(!debug)
                    hitbox.isActive = true;
            }
            else
            {
                if(activeHitboxes.Contains(hitbox))
                    activeHitboxes.Remove(hitbox);
                if(!debug)
                    hitbox.isActive = false;
            }
        }
    }

    private void UpdateCombatData(object move)
    {
        foreach (CombatBox hitbox in hitboxes)
        {
            hitbox.SetCombatData(TimeManager.Frame);
        }
    }

    void OnDrawGizmosSelected()
    {
        DrawBoxes(ref hitboxes);
        DrawBoxes(ref hurtboxes);
    }

    private void DrawBoxes(ref CombatBox[] boxes)
    {
        foreach (CombatBox box in boxes)
        {
            if (!box.isActive) return;
            switch (box.type)
            {
                case BoxType.Hit:
                    Gizmos.color = Color.red;
                    break;
                case BoxType.Hurt:
                    Gizmos.color = Color.green;
                    break;
                case BoxType.Throw:
                    Gizmos.color = Color.blue;
                    break;
                case BoxType.Projection:
                    Gizmos.color = Color.yellow;
                    break;
                default:
                    break;
            }
            box.SetParent(transform);
            box.UpdateBox();
            DrawBox(box.worldBox);
        }
    }

    private void DrawBox(Box box)
    {
        Vector2 bottomLeft = new Vector2(box.center.x - box.size.x / 2f,  box.center.y - box.size.y / 2f);
        Vector2 bottomRight = new Vector2(box.center.x + box.size.x / 2f,  box.center.y - box.size.y / 2f);
        Vector2 topRight = new Vector2(box.center.x + box.size.x / 2f,  box.center.y + box.size.y / 2f);
        Vector2 topLeft = new Vector2(box.center.x - box.size.x / 2f,  box.center.y + box.size.y / 2f);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}

[System.Serializable]
public struct Box
{
    public Vector2 center;
    public Vector2 size;
}

[System.Serializable]
public class CombatBox
{
    public BoxType type;
    public Box box;
    public Box worldBox{ get; private set; }
    public CombatData combatData;
    public bool isActive;

    private Transform parent;

    public int HitCount { get; private set; } = 0;

    public CombatBox(Transform parent, CombatBox combatBox)
    {
        this.parent = parent;
        type = combatBox.type;
        box = combatBox.box;
        worldBox = combatBox.worldBox;
        combatData = combatBox.combatData;
        isActive = combatBox.isActive;
        UpdateBox();
    }

    public void SetCombatData(int frame)
    {
        combatData.StartFrame = frame;
        HitCount = 1;
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
    }

    public void SetHitCount(int hitCount)
    {
        HitCount = hitCount;
    }

    public void UpdateBox()
    {
        worldBox = ToWorld();
    }
    
    public Box BoxLocal(int frame)
    {
        int elapse = frame - combatData.StartUpEnds;
        
        if (elapse < 0 || elapse >= combatData.Positions.Length) return box;

        int index = elapse >= combatData.Positions.Length ? combatData.Positions.Length - 1 : elapse;
        
        Box position = combatData.Positions[index];
        
        box.center = position.center;
        box.size = position.size;
        
        return box;
    }

    public Box ToWorld(int frame = 0)
    {
        Box local = BoxLocal(frame);
        if (!parent)
            return local;
        float alignment = Vector3.Dot(parent.right, Vector3.right);
        Vector2 flipped = alignment < -0.7f ? 
            new Vector2(-local.center.x, local.center.y) :
            local.center;
        
        Vector2 center = (Vector2)parent.position + flipped;
        Vector2 size = local.size;
        
        return new Box
        {
            center = center,
            size = size,
        };
    }
}

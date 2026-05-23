using System;
using UnityEngine;

public class PlayerBoxes : MonoBehaviour
{
    [Range(1, 2)] public int PlayerIndex;

    public int PresetIndex;
    private int currPresetIndex;

    private Boxes activeBoxes;
    public Moveset currMove { get; private set; }
    public int activeHits { get; private set; }

    private int presetIndex
    {
        get => currPresetIndex;
        set
        {
            if (currPresetIndex == value) return;
            currPresetIndex = value;
            UpdateBoxes();
        }
    }
    
    private void Start()
    {
        switch (PlayerIndex)
        {
            case 1:
                BoxesResolver.i.player1 = this;
                break;
            case 2:
                BoxesResolver.i.player2 = this;
                break;
        }
    }
    
    private void Update()
    {
        presetIndex = PresetIndex;
        
    }

    private void SetActiveBoxes(object index)
    {
        PresetIndex = (int)index;
        presetIndex = PresetIndex;
    }

    private void UpdateBoxes()
    {
        if (currPresetIndex <= 0 || currPresetIndex > BoxesResolver.i.PresetData.Presets.Length)
        {
            activeBoxes = null;
        }
        else
        {
            Boxes currentBoxes = BoxesResolver.i.PresetData.Presets[currPresetIndex - 1];
            activeBoxes = currentBoxes;
        }
    }

    public void SetActiveHits(object hits)
    {
        activeHits = (int)hits;
    }

    private void SetCurrentMove(object move)
    {
        currMove = (Moveset)move;
    }

    public Boxes ToWorld()
    {
        if (activeBoxes == null) return null;

        Box[] worldHitboxes = new Box[activeBoxes.hitboxes.Length];
        Box[] worldHurtboxes = new Box[activeBoxes.hurtboxes.Length];
        Box[] worldProximityboxes = new Box[activeBoxes.proximityboxes.Length];

        if (activeBoxes.hitboxes.Length > 0)
            for(int i = 0; i < activeBoxes.hitboxes.Length; i++)
            {
                worldHitboxes[i] = activeBoxes.hitboxes[i].ToWorld(transform);
            }

        if (activeBoxes.hurtboxes.Length > 0) 
            for(int i = 0; i < activeBoxes.hurtboxes.Length; i++)
            {
                worldHurtboxes[i] = activeBoxes.hurtboxes[i].ToWorld(transform);
            }
        
        if (activeBoxes.proximityboxes.Length > 0) 
            for(int i = 0; i < activeBoxes.proximityboxes.Length; i++)
            {
                worldProximityboxes[i] = activeBoxes.proximityboxes[i].ToWorld(transform);
            }

        return new Boxes
        {
            hitboxes = worldHitboxes,
            hurtboxes = worldHurtboxes,
            proximityboxes = worldProximityboxes
        };
    }

    private void OnEnable()
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Subscribe("p1_attack", SetCurrentMove);
                EventBus.Subscribe("p1_update_boxes", SetActiveBoxes);
                EventBus.Subscribe("p1_update_hits", SetActiveHits);
                break;
            case 2:
                EventBus.Subscribe("p2_attack", SetCurrentMove);
                EventBus.Subscribe("p2_update_boxes", SetActiveBoxes);
                EventBus.Subscribe("p2_update_hits", SetActiveHits);
                break;
        }
    }
    private void OnDisable()
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Unsubscribe("p1_attack", SetCurrentMove);
                EventBus.Unsubscribe("p1_update_boxes", SetActiveBoxes);
                EventBus.Unsubscribe("p1_update_hits", SetActiveHits);
                break;
            case 2:
                EventBus.Unsubscribe("p2_attack", SetCurrentMove);
                EventBus.Unsubscribe("p2_update_boxes", SetActiveBoxes);
                EventBus.Unsubscribe("p2_update_hits", SetActiveHits);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        DrawBoxes();
    }

    private void DrawBoxes()
    {
        Boxes worldBoxes = ToWorld();
        if (worldBoxes == null) return;
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);
        foreach (Box box in worldBoxes.proximityboxes)
        {
            Vector3 center = new Vector3(box.center.x, box.center.y, -1f);
            Gizmos.DrawCube(center, box.size);
        }
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        foreach (Box box in worldBoxes.hitboxes)
        {
            Vector3 center = new Vector3(box.center.x, box.center.y, -1f);
            Gizmos.DrawCube(center, box.size);
        }
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
        foreach (Box box in worldBoxes.hurtboxes)
        {
            Vector3 center = new Vector3(box.center.x, box.center.y, -1f);
            Gizmos.DrawCube(center, box.size);
        }
    }
}

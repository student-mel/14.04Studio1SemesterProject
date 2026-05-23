using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BoxData", menuName = "Scriptable Objects/Box Data")]
public class BoxData : ScriptableObject
{
    [Tooltip("Note:\nBoxes positions should match character animations facing the right only")]
    public Boxes[] Presets;

    public static Boxes currentPreview;
    private void OnValidate()
    {
        Boxes needsUpdate = null;
        foreach (Boxes preset in Presets)
        {
            int index = Array.IndexOf(Presets, preset) + 1;
            preset.name = $"Preset {index}: {preset.presetName}";
            preset.OnValidate();
            
            if(preset.needsUpdate)
                needsUpdate = preset;
        }

        if (needsUpdate == null) return;
        foreach (Boxes preset in Presets)
        {
            preset.needsUpdate = false;
            if (preset == needsUpdate) continue;
            preset.Preview = false;
        }
    }
}
[System.Serializable]
public class Boxes
{
    [HideInInspector] public string name;
    
    public string presetName;

    public bool Preview;
    private bool preview;
    
    [HideInInspector]public bool needsUpdate;
    public bool _preview
    {
        get => preview;
        set
        {
            if (preview == value) return;
            preview = value;
            if(preview)
                needsUpdate = true;
        }
    }

    public Box[] hitboxes;
    public Box[] hurtboxes;
    public Box[] proximityboxes;

    public void OnValidate()
    {
        _preview = Preview;
        foreach (Box hitbox in hitboxes)
        {
            int index = Array.IndexOf(hitboxes, hitbox) + 1;
            hitbox.name = $"Hitbox {index}";
        }
        foreach (Box hurtbox in hurtboxes)
        {
            int index = Array.IndexOf(hurtboxes, hurtbox) + 1;
            hurtbox.name = $"Hurtbox {index}";
        }
        foreach (Box proximitybox in proximityboxes)
        {
            int index = Array.IndexOf(proximityboxes, proximitybox) + 1;
            proximitybox.name = $"Proximitybox {index}";
        }
    }
}
[System.Serializable]
public class Box
{
    [HideInInspector] public string name;
    
    public Vector2 center;
    public Vector2 size;
    
    public Box ToWorld(Transform parent)
    {
        if (!parent)
            return this;
        
        float alignment = Vector3.Dot(parent.right, Vector3.right);
        Vector2 flipped = alignment < -0.7f ? 
            new Vector2(-center.x, center.y) :
            center;
        
        Vector2 newCenter = (Vector2)parent.position + flipped;
        Vector2 newSize = size;

        return new Box
        {
            center = newCenter,
            size = newSize,
        };
    }
}

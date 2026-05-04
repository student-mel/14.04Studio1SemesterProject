using System;
using UnityEngine;

public class BoxViewer : MonoBehaviour
{
    public BoxData PresetData;

    private Boxes[] presets;

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        presets = PresetData.Presets;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying) return;
        foreach (Boxes preset in presets)
        {
            if (!preset.Preview) continue;
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            foreach (Box box in preset.hitboxes)
            {
                Gizmos.DrawCube(box.center, box.size);
            }
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            foreach (Box box in preset.hurtboxes)
            {
                Gizmos.DrawCube(box.center, box.size);
            }
        }
    }
}

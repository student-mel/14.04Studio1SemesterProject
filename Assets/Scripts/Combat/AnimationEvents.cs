using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [Range(1, 2)] public int PlayerIndex;

    private string player;

    private void Awake()
    {
        player = $"p{PlayerIndex}";
    }

    public void UpdateFightingBoxes(int presetIndex)
    {
        EventBus.Emit($"{player}_update_boxes", presetIndex);
    }
    public void UpdateMovesetHits(int hits)
    {
        EventBus.Emit($"{player}_update_hits", hits);
    }
    public void OnAttackEnded()
    {
        EventBus.Emit($"{player}_attack_ended", null);
    }
}

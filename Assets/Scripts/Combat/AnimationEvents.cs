using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [Range(1, 2)] public int PlayerIndex;

    public void UpdateFightingBoxes(int presetIndex)
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_update_boxes", presetIndex);
                break;
            case 2:
                EventBus.Emit("p2_update_boxes", presetIndex);
                break;
        }
    }
    public void UpdateMovesetHits(int hits)
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_update_hits", hits);
                break;
            case 2:
                EventBus.Emit("p2_update_hits", hits);
                break;
        }
    }

    public void OnAttackEnded()
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_attack_ended", null);
                break;
            case 2:
                EventBus.Emit("p2_attack_ended", null);
                break;
        }
    }
}

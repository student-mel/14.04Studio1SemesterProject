using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_StreaksCounter : MonoBehaviour
{
    private int[] streaks = new int[2];
    private TextMeshProUGUI[] textMeshes;

    private bool anticipateHit1 = false, anticipateHit2 = false;
    
    private void OnEnable()
    {
        EventBus.Subscribe("actionResult", StartAnticipate);
        EventBus.Subscribe("hit_result", OnActionResult);
        EventBus.Subscribe("attack_finished", EndAnticipate);
    }

    private void EndAnticipate(object obj)
    {
        if (anticipateHit1 && (int)obj == 0)
        {
            OnActionResult(new PlayerResult((int)obj, ""));
        }

        if (anticipateHit2 && (int)obj == 1)
        {
            OnActionResult(new PlayerResult((int)obj, ""));
        }
    }

    private void StartAnticipate(object obj)
    {
        PlayerResult playerResult = (PlayerResult)obj;
        anticipateHit1 = playerResult.Index == 0;
        anticipateHit2 = playerResult.Index == 1;
    }

    private void Awake()
    {
        textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("hit_result",  OnActionResult);
        EventBus.Unsubscribe("actionResult",  OnActionResult);
        EventBus.Unsubscribe("attack_finished", EndAnticipate);
    }

    private void OnActionResult(object obj)
    {
        PlayerResult result = (PlayerResult)obj;
        if (result.Index == 0)
            anticipateHit1 = false;
        else
            anticipateHit2 = false;
        
        if (result.ToString().Equals("Miss") || !result.IsHit)
            streaks[result.Index] = 0;
        else
            streaks[result.Index]++;
        
        textMeshes[result.Index].text = $"Streak: {streaks[result.Index]}";
    }
}

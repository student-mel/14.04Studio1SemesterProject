using TMPro;
using UnityEngine;

public class UI_StreaksCounter : MonoBehaviour
{
    private int[] streaks = new int[2];
    private TextMeshProUGUI[] textMeshes;
    
    private void OnEnable()
    {
        EventBus.Subscribe("actionResult", OnActionResult);
    }

    private void Awake()
    {
        textMeshes = GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("actionResult",  OnActionResult);
    }

    private void OnActionResult(object obj)
    {
        PlayerResult result = (PlayerResult)obj;
        
        if (result.ToString().Equals("Miss"))
            streaks[result.Index] = 0;
        else
            streaks[result.Index]++;
        
        textMeshes[result.Index].text = $"Streak: {streaks[result.Index]}";
    }
}

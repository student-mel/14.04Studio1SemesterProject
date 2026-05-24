using System;
using TMPro;
using UnityEngine;

public class PlayerStreakDisplay : MonoBehaviour
{
    public int player = 1;
    TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe($"p{player}_add_streak", UpdateStreak);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe($"p{player}_add_streak", UpdateStreak);
    }

    private void UpdateStreak(object obj)
    {
        text.text = obj.ToString();
    }
}

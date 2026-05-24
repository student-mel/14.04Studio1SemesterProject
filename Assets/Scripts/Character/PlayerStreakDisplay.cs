using System;
using TMPro;
using UnityEngine;

public class PlayerStreakDisplay : MonoBehaviour
{
    public int player = 1;
    [SerializeField]TextMeshProUGUI[] text;

    private void Awake()
    {
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
        text[0].text = obj.ToString();
        text[1].text = 
    }
}

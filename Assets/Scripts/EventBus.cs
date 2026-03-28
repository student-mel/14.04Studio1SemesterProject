using System;
using System.Collections.Generic;

/// <summary>
/// Small custom event messages router
/// </summary>
public static class EventBus
{
    private static Dictionary<string, Action<object>> events = new();

    public static void Subscribe(string eventName, Action<object> listener)
    {
        if (!events.ContainsKey(eventName))
            events[eventName] = delegate { };

        events[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action<object> listener)
    {
        if (events.ContainsKey(eventName))
            events[eventName] -= listener;
    }

    public static void Emit(string eventName, object data = null)
    {
        if (events.ContainsKey(eventName))
            events[eventName]?.Invoke(data);
    }
}
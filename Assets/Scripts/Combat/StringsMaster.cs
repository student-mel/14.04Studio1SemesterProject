using System;
using UnityEngine;

public class StringsMaster : MonoBehaviour
{
    public static bool p1CanString = false;
    public static bool p2CanString = false;

    private void Update()
    {
        Debug.Log($"p1CanString: {p1CanString}");
        Debug.Log($"p2CanString: {p2CanString}");
    }

    private void OnEnable()
    {
        EventBus.Subscribe("p2_hurt", Player1WindowOpen);
        EventBus.Subscribe("p1_hurt", Player2WindowOpen);
        EventBus.Subscribe("p1_end_strings_window", Player1WindowClose);
        EventBus.Subscribe("p2_end_strings_window", Player2WindowClose);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("p2_hurt", Player1WindowOpen);
        EventBus.Unsubscribe("p1_hurt", Player2WindowOpen);
        EventBus.Unsubscribe("p1_end_strings_window", Player1WindowClose);
        EventBus.Unsubscribe("p2_end_strings_window", Player2WindowClose);
    }

    private void Player1WindowOpen(object move)
    {
        p1CanString = true;
    }
    private void Player2WindowOpen(object move)
    {
        p2CanString = true;
    }
    private void Player1WindowClose(object nothing)
    {
        p1CanString = false;
    }
    private void Player2WindowClose(object nothing)
    {
        p2CanString = false;
    }
}

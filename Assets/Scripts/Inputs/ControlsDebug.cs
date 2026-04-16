using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlsDebug : MonoBehaviour
{
    public Image[] Buttons;

    public Color Default;
    public Color Pressed;

    [Range(1, 2)] public int index = 1;

    private void OnEnable()
    {
        if (index == 1)
        {
            EventBus.Subscribe("on_p1_directional_input", OnDirectionPressed);
            EventBus.Subscribe("on_p1_directional_input_cancelled", OnDirectionReleased);
            EventBus.Subscribe("on_p1_attack_input", OnAttackPressed);
            EventBus.Subscribe("on_p1_attack_input_cancelled", OnAttackReleased);
        }
        else if (index == 2)
        {
            EventBus.Subscribe("on_p2_directional_input", OnDirectionPressed);
            EventBus.Subscribe("on_p2_directional_input_cancelled", OnDirectionReleased);
            EventBus.Subscribe("on_p2_attack_input", OnAttackPressed);
            EventBus.Subscribe("on_p2_attack_input_cancelled", OnAttackReleased);
        }
    }
    private void OnDisable()
    {
        if (index == 1)
        {
            EventBus.Unsubscribe("on_p1_directional_input", OnDirectionPressed);
            EventBus.Unsubscribe("on_p1_directional_input_cancelled", OnDirectionReleased);
            EventBus.Unsubscribe("on_p1_attack_input", OnAttackPressed);
            EventBus.Unsubscribe("on_p1_attack_input_cancelled", OnAttackReleased);
        }
        else if (index == 2)
        {
            EventBus.Unsubscribe("on_p2_directional_input", OnDirectionPressed);
            EventBus.Unsubscribe("on_p2_directional_input_cancelled", OnDirectionReleased);
            EventBus.Unsubscribe("on_p2_attack_input", OnAttackPressed);
            EventBus.Unsubscribe("on_p2_attack_input_cancelled", OnAttackReleased);
        }
    }

    private void OnDirectionPressed(object input)
    {
        int i = (int)input;
        Buttons[i].color = Pressed;

        for (int b = 0; b < 8; b++)
        {
            if (i == b) continue;
            Buttons[b].color = Default;
        }
    }

    private void OnDirectionReleased(object nothing)
    {
        for (int b = 0; b < 8; b++)
            Buttons[b].color = Default;
    }    
    private void OnAttackPressed(object input)
    {
        int i = (int)input;
        Buttons[i].color = Pressed;
    }
    private void OnAttackReleased(object input)
    {
        int i = (int)input;
        Buttons[i].color = Default;
    }
}

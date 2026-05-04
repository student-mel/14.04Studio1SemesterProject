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
            EventBus.Subscribe("p1_dirinput", OnDirectionPressed);
            EventBus.Subscribe("p1_dirinput_cancelled", OnDirectionReleased);
            EventBus.Subscribe("p1_attackinput", OnAttackPressed);
            EventBus.Subscribe("p1_attackinput_cancelled", OnAttackReleased);
        }
        else if (index == 2)
        {
            EventBus.Subscribe("p2_dirinput", OnDirectionPressed);
            EventBus.Subscribe("p2_dirinput_cancelled", OnDirectionReleased);
            EventBus.Subscribe("p2_attackinput", OnAttackPressed);
            EventBus.Subscribe("p2_attackinput_cancelled", OnAttackReleased);
        }
    }
    private void OnDisable()
    {
        if (index == 1)
        {
            EventBus.Unsubscribe("p1_dirinput", OnDirectionPressed);
            EventBus.Unsubscribe("p1_dirinput_cancelled", OnDirectionReleased);
            EventBus.Unsubscribe("p1_attackinput", OnAttackPressed);
            EventBus.Unsubscribe("p1_attackinput_cancelled", OnAttackReleased);
        }
        else if (index == 2)
        {
            EventBus.Unsubscribe("p2_dirinput", OnDirectionPressed);
            EventBus.Unsubscribe("p2_dirinput_cancelled", OnDirectionReleased);
            EventBus.Unsubscribe("p2_attackinput", OnAttackPressed);
            EventBus.Unsubscribe("p2_attackinput_cancelled", OnAttackReleased);
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

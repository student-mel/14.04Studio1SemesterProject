using System.Collections.Generic;
using UnityEngine;

public class InputBuffer
{
    List<BufferedInput> inputs = new List<BufferedInput>();

    public void AddInput(CombatActionType _action)
    {
        inputs.Add(new BufferedInput
        {
            action = _action,
            timeStamp = Time.time
        });
    }
    public BufferedInput? GetInputForBeat(float _beatTime, float _window)
    {
        foreach (BufferedInput input in inputs)
        {
            if(Mathf.Abs(input.timeStamp - _beatTime) <= _window)
                return input;
        }
        return null;
    }

    public void ClearPastInputs(float time)
    {
        inputs.RemoveAll(i => i.timeStamp < time);
    }
}

public struct BufferedInput
{
    public CombatActionType action;
    public float timeStamp;
}

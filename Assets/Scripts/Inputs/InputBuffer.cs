using System.Collections.Generic;
using UnityEngine;

public class InputBuffer
{
    List<BufferedInput> inputs = new List<BufferedInput>();

    public CombatIntent playerIntent;

    public int id;

    public void AddInput(CombatActionType _action, float _time)
    {
        inputs.Add(new BufferedInput
        {
            action = _action,
            timeStamp = _time
        });
    }
    public BufferedInput? GetInputForBeat(float _beatTime)
    {
        foreach (BufferedInput input in inputs)
        {
            if(Mathf.Abs(input.timeStamp - _beatTime) <= TestBeatClock.Interval * 0.5f)
                return input;
        }
        return null;
    }

    public CombatIntent GetIntentForBeat(float _beatTime)
    {
        BufferedInput? _input = GetInputForBeat(_beatTime);

        playerIntent.id = id;

        if (!_input.HasValue)
        {
            playerIntent.action = CombatActionType.None;
            playerIntent.beatTime = _beatTime;
            playerIntent.timingOffset = 10;

            return playerIntent;
        }

        BufferedInput currInput = (BufferedInput)_input;

        playerIntent.action = currInput.action;
        playerIntent.beatTime = _beatTime;
        playerIntent.timingOffset = currInput.timeStamp - _beatTime;

        inputs.Remove(currInput);

        return playerIntent;
    }

    public void ClearExpiredInputs(float time)
    {
        inputs.RemoveAll(i => i.timeStamp < (time - TestBeatClock.Interval));
    }
}

public struct BufferedInput
{
    public CombatActionType action;
    public float timeStamp;
}

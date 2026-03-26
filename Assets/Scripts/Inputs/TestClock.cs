using System;
using UnityEngine;

public class TestClock : TickSystem
{
    public Action CustomUpdate;

    public TestClock(float _interval) : base(_interval) { }

    protected override void Tick()
    {
        CustomUpdate?.Invoke();
    }
}


using System;
using UnityEngine;

public class FrameClock : MonoBehaviour
{
    public static int Frame {  get; private set; }

    public static void Tick()
    {
        Frame++;
    }
    private void Update()
    {
        Tick();
    }
}

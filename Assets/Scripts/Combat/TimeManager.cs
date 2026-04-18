using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static int Frame = 0;
    private const float FRAMES_PER_SECOND = 60f;
    private float frameDuration =  1f / FRAMES_PER_SECOND;

    private float accumulator = 0;
    private void Update()
    {
        accumulator += Time.deltaTime;

        while (accumulator >= frameDuration)
        {
            Frame++;
            EventBus.Emit("fixed_game_update", Frame);
            accumulator -= frameDuration;
        }
    }
}


using System;
using UnityEngine;

public class CombatBoxManager : MonoBehaviour
{
    public TimingWindows testingWindows;
    public CombatBox hitboxes;

    private void OnEnable()
    {
        EventBus.Subscribe("fixed_game_update", FixedGameUpdate);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe("fixed_game_update", FixedGameUpdate);
    }

    private void FixedGameUpdate(object frame)
    {
        ActivateHitboxesForFrame();
    }

    private void ActivateHitboxesForFrame()
    {
        
    }
}

public enum BoxType
{
    Hit,
    Hurt,
    Throw,
    Projection
}

[System.Serializable]
public class TimingWindows
{
    public int startUp;
    public int active;
    public int recover;

    public int chainStart;
    public int chainEnd;
}
[System.Serializable]
public class CombatData
{
    public int StartFrame;
    public TimingWindows Windows;
    [Tooltip("An array of translations for the hitbox during the active frames")]
    public Translation[] Translations;

    public int StartUpStart => StartFrame + 1;
    public int StartUpEnds => StartFrame + Windows.startUp;
    public int ActiveStarts => StartUpEnds + 1;
    public int ActiveEnds => StartUpEnds + Windows.active;
    public int RecoverStarts => ActiveEnds + 1;
    public int RecoverEnds => ActiveEnds + Windows.recover;
    
    public int ChainStart => StartFrame + Windows.chainStart;
    public int ChainEnd => StartFrame + Windows.chainEnd;
}

[System.Serializable]
public class Translation
{
    public Vector2 center;
    public Vector2 size;
    [Tooltip("Z Rotation")]
    public float rotation;
    [Tooltip("position value from 0 to 1")]
    public float t;
}

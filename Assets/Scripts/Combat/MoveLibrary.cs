using UnityEngine;

[CreateAssetMenu(fileName = "MoveLibrary", menuName = "Scriptable Objects/MoveLibrary")]
public class MoveLibrary : ScriptableObject
{
    public Move[] MoveList;
}

[System.Serializable]
public class Move
{
    [Tooltip("Name should match with animator parameters")]
    public string Name;
    public MoveType moveType;
    public InputType[] moveString;

    [HideInInspector] public int priority;

    // public float TotalBeats;
    //
    // public float StartupBeats;
    // public float ActiveBeats;
    // public float RecoveryBeats;
    //
    // public float ChainStartBeat;
    // public float ChainEndBeat;
}

public enum MoveType
{
    Movement,
    Attack
}

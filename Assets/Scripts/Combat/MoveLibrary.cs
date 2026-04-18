using UnityEngine;

[CreateAssetMenu(fileName = "MoveLibrary", menuName = "Scriptable Objects/MoveLibrary")]
public class MoveLibrary : ScriptableObject
{
    public CharacterMove[] MoveList;
}

[System.Serializable]
public class CharacterMove
{
    [Tooltip("Name should match with animator parameters")]
    public string Name;
    [Tooltip("Damage = Attack > Move = Movement\nPlease make sure this label is correct for MoveMaster to send the correct data")]
    public MoveType moveType;
    [Tooltip("Inputs in sequence for this move to be valid")]
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

using UnityEngine;

[CreateAssetMenu(fileName = "MoveLibrary", menuName = "Scriptable Objects/MoveLibrary")]
public class MoveLibrary : ScriptableObject
{
    public Moveset[] MoveList;
}

[System.Serializable]
public class Moveset
{
    public string Name;
    [Tooltip("Damage = Attack > Move = Movement\nPlease make sure this label is correct for MoveMaster to send the correct data")]
    public MoveType moveType;
    [Tooltip("Inputs in sequence for this move to be valid")]
    public InputType[] inputString;

    [HideInInspector] public int priority;
}

public enum MoveType
{
    Movement,
    Attack
}

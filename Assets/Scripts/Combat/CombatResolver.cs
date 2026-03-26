using UnityEngine;

public class CombatResolver : MonoBehaviour
{

}

public enum CombatActionType
{
    None,
    LightAttack,
    HeavyAttack,
    Block
}

public struct CombatIntent
{
    public int id;
    public CombatActionType action;
    public int beatIndex;
    public float timingOffset;
}

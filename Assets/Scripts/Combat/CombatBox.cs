using System;
using UnityEngine;

public class CombatBox : MonoBehaviour
{
    public Box box;
    public BoxType boxType;

    public CombatData combatData;

    private void Awake()
    {
        gameObject.name = $"{boxType.ToString()}Box";
    }
}

public struct Box
{
    public Vector2 center;
    public Vector2 size;
}
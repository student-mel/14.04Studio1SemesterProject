using System;
using Character;
using UnityEngine;

public class TempHitBoxHandler : MonoBehaviour
{
    private PlayerController pc;
    public int playerNum = 1;

    private void Awake()
    {
        //pc = transform.parent.GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Hit" + other.name);
        EventBus.Emit($"p{playerNum}_hurt", gameObject.name);
    }
}

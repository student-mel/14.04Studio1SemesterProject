using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
public class Hitbox : MonoBehaviour
{
    public float HitboxStartEdge;
    public float HitboxWidth;
    public float HitboxHeight;

    public bool facingLeft;

    Vector3 direction;
    Vector3 center;
    Vector3 size;

    BoxCollider hitbox;

    public void EnableHitbox() => hitbox.enabled = true;
    public void DisableHitbox() => hitbox.enabled = false;

    public bool debug;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider>();
        hitbox.isTrigger = true;
        EnableHitbox();
    }

    private void Update()
    {
        UpdateHitbox();
        hitbox.center = center;
        hitbox.size = size;
    }

    private void UpdateHitbox()
    {
        direction = (facingLeft ? Vector3.left : Vector3.right);
        center = direction * (HitboxStartEdge + HitboxWidth / 2f);
        size = new Vector3(HitboxWidth, HitboxHeight, 0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerTriggerStay(other);
    }

    private void PlayerTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        Debug.Log("Hit!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        if (Application.isPlaying && debug)
        {
            Gizmos.DrawWireCube(center, size);
        }
    }
}


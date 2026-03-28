using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent (typeof(BoxCollider))]
public class TestingHitbox : MonoBehaviour
{
    [Range(1, 2)] public int Index;
    public float HitboxStartEdge;
    public float HitboxWidth;
    public float HitboxHeight;

    public bool facingLeft;

    Vector3 direction;
    Vector3 center;
    Vector3 size;

    BoxCollider hitbox;

    PlayerInputHandler player;

    public void EnableHitbox() => hitbox.enabled = true;
    public void DisableHitbox() => hitbox.enabled = false;

    public bool debug;

    private void Awake()
    {
        hitbox = GetComponent<BoxCollider>();
        hitbox.isTrigger = true;
        DisableHitbox();
    }

    private void Start()
    {
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (player == null)
        {
            PlayerInputHandler[] players = FindObjectsByType<PlayerInputHandler>(FindObjectsSortMode.None);
            player = players.FirstOrDefault(p => p.PlayerIndex == Index);

            player.AttackEvent += LightAttack;
        }
    }

    private void Update()
    {
        UpdateHitbox();
    }

    private void UpdateHitbox()
    {
        direction = (facingLeft ? Vector3.left : Vector3.right);
        center = direction * (HitboxStartEdge + HitboxWidth / 2f);
        size = new Vector3(HitboxWidth, HitboxHeight, 0.5f);

        hitbox.center = center;
        hitbox.size = size;
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerTriggerStay(other);
    }

    private void PlayerTriggerStay(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        List<AuthorisedAttack> aAttacks = CombatResolver.i.authorisedAttacks;
        AuthorisedAttack hitAttack;
        foreach (AuthorisedAttack attack in aAttacks)
        {
            if(!attack.hitApplied && attack.id == Index && attack.expiryTime < Time.time)
            {
                hitAttack = attack;
                break;
            }
        }

        Debug.Log($"Player {Index}'s attack hits");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        if (Application.isPlaying && debug)
        {
            Gizmos.DrawWireCube(center, size);
        }
    }

    Coroutine AttackRoutine;

    public void LightAttack()
    {
        if (AttackRoutine != null) return;
        AttackRoutine = StartCoroutine(ExecuteAttack());
    }

    IEnumerator ExecuteAttack()
    {
        DisableHitbox();

        yield return new WaitForEndOfFrame();

        player.LockAction();

        yield return new WaitForSeconds(TestBeatClock.Interval * 0.2f); //Startup

        UpdateHitbox();
        EnableHitbox();

        yield return new WaitForSeconds(TestBeatClock.Interval * 0.3f); //Active Frame

        DisableHitbox();

        yield return new WaitForSeconds(TestBeatClock.Interval * 0.1f); //Recovery

        AttackRoutine = null;
        player.UnlockAction();
    }
}


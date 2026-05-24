using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHitFeedbackManager : MonoBehaviour
{
    public GameObject[] PlayerHitFeedback;
    private readonly Dictionary<string, int> _attackType = new()
    {
        { "Light Attack", 0},
        { "Medium Attack", 1},
        { "Heavy Attack", 2 }
    };
    
    public bool IsOnCooldown { get; private set; }

    public IEnumerator Cooldown(float duration)
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(duration);
        IsOnCooldown = false;
    }


    private void OnEnable()
    {
        EventBus.Subscribe("p1_hurt_point", SpawnHurt);
        EventBus.Subscribe("p2_hurt_point", SpawnHurt);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("p1_hurt_point", SpawnHurt);
        EventBus.Unsubscribe("p2_hurt_point", SpawnHurt);
    }

    private void SpawnHurt(object obj)
    {
        if (IsOnCooldown) return;
        StartCoroutine(Cooldown(0.4f));
        
        Box box = (Box)obj;
        Vector2 spawnPosition = box.center;
        Instantiate(PlayerHitFeedback[Random.Range(0, 3)],  spawnPosition, Quaternion.identity);
    }
    
   
}

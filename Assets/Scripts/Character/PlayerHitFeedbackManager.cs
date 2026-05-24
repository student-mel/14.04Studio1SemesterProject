using System;
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

    private void OnEnable()
    {
        EventBus.Subscribe("p1_hurt_point", SpawnHurt);
        EventBus.Subscribe("p2_hurt_point", SpawnHurt);
    }

    private void SpawnHurt(object obj)
    {
        Box box = (Box)obj;
        Vector2 spawnPosition = box.center;
        Instantiate(PlayerHitFeedback[Random.Range(0, 3)],  spawnPosition, Quaternion.identity);
    }
    
   
}

using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using UnityEngine;
using UnityEngine.UI;

public class healthBarUI : MonoBehaviour
{
    public float Health, MaxHealth, Width, Height;
    [SerializeField]
    private RectTransform healthBar;

    public PlayerController.PlayerEnum playerEnum;

    private void OnEnable()
    {
        string p = $"p{(int)playerEnum + 1}_";
        EventBus.Subscribe("set_maxhealth", setMaxHealth);
        EventBus.Subscribe($"{p}set_currenthealth", updateHealth);
    }

    private void OnDisable()
    {
        string p = $"p{(int)playerEnum + 1}_";
        EventBus.Unsubscribe("set_maxhealth", setMaxHealth);
        EventBus.Unsubscribe($"{p}set_currenthealth", updateHealth);
    }

    private void setMaxHealth(object maxHealth)
    {
        MaxHealth = (float)maxHealth;
    }

    private void subtractHealth(float healthLost)
    {
        Health = Health - healthLost;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }

    private void updateHealth(object newHealth)
    {
        Health = (float)newHealth;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }

    void Start()
    {

    }

    void Update()
    {
        subtractHealth(0f); //Is this meant to be .1, drains health but if its part of the rhythm then makes sense
    }
}

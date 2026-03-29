using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBarUI : MonoBehaviour
{
    public float Health, MaxHealth, Width, Height;
    [SerializeField]
    private RectTransform healthBar;

    public void setMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void subtractHealth(float healthLost)
    {
        Health = Health - healthLost;
        float newWidth = (Health / MaxHealth) * Width;

        healthBar.sizeDelta = new Vector2(newWidth, Height);
    }

    public void updateHealth(float newHealth)
    {
        Health = newHealth;
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

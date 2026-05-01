using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; //HP Stat
    public int currentHealth;

    public Transform spawnPoint;

    public System.Action OnHit;
    public System.Action OnDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        OnHit?.Invoke();
        AudioManager.Instance?.PlayHit();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath?.Invoke();
        }
    }

    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        transform.position = spawnPoint.position;
    }
}

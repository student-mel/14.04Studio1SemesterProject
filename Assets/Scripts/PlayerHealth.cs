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
        Debug.Log(gameObject.name + " TakeDamage called");
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        OnHit?.Invoke();
        AudioManager.Instance?.PlayHit(gameObject);

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

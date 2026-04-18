using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float dmg);
    void Die();
    
    float MaxHealth { get; }
    float CurrentHealth { get; set; }
}

using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private bool hasHit;
    private Health ownerHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ownerHealth = GetComponentInParent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        hasHit = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hitbox touched: " + other.name);

            if (hasHit) return;

        Hurtbox hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox == null) return;

        Health health = other.GetComponentInParent<Health>();
        if (health == null) return;

        if (health == ownerHealth) return;

        health.TakeDamage(damage);
        hasHit = true;
    }
}

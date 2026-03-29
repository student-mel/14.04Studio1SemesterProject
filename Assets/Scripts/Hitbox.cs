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
        
        health.TakeDamage(damage * GetDamageMult(RhythmStore.Instance.result));
        hasHit = true;
    }

    float GetDamageMult(string result)
    {
        switch (result)
        {
            case "Perfect":
                return 1.75f;
            case "Great":
                return 1.3f;
            case "Good":
                return 1.15f;
            case "Syncopated":
                return 2f;
            case "Miss":
                return 0.5f;
        }
        
        return 1f;
    }
}

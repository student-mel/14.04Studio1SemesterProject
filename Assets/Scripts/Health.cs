using UnityEngine;

using UnityEngine.InputSystem;
using System.Collections;
public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.13f;
    [SerializeField] private int flashCount = 3;
    [SerializeField] private Animator animator;

    public float currentHealth;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private Movement movement;

    private Vector3 spawnPosition;

    [SerializeField] private healthBarUI healthUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;

        if (healthUI != null)
        {
            /*healthUI.setMaxHealth(maxHealth);
            healthUI.updateHealth(currentHealth);*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<Movement>();
        spawnPosition = transform.position;

    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + " TakeDamage called");
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log(gameObject.name + " HP: " + currentHealth);

        /*if (healthUI != null)
            healthUI.updateHealth(currentHealth);
            */

        AudioManager.Instance?.PlayHit(gameObject);

        movement.StartHurt();
        animator.ResetTrigger("isHit");
        animator.SetTrigger("isHit");

        if (spriteRenderer != null)
        {
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                spriteRenderer.color = originalColor;
            }

            flashCoroutine = StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = flashColor;
            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        spriteRenderer.color = originalColor;
        flashCoroutine = null;
    }

    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        transform.position = spawnPosition;

        /*if (healthUI != null)
            healthUI.updateHealth(currentHealth);*/
    }
}

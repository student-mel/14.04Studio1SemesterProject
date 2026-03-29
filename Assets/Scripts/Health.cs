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

    private int currentHealth;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private Movement movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
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

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHealth);

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
}

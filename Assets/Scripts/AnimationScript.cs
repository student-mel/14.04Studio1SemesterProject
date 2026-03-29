using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference heavyAttackAction;
    [SerializeField] private Animator animator;
    [SerializeField] private Movement movement;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnEnable()
    {
        attackAction.action.Enable();
        attackAction.action.performed += OnAttack;

        heavyAttackAction.action.Enable();
        heavyAttackAction.action.performed += OnHeavyAttack;
    }

    private void OnDisable()
    {
        attackAction.action.performed -= OnAttack;
        attackAction.action.Disable();

        heavyAttackAction.action.performed -= OnHeavyAttack;
        heavyAttackAction.action.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (movement != null)
        {
            movement.StartLightAttack();
        }
    }

    private void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (movement != null)
            movement.StartHeavyAttack();
    }

    public void EndAttack()
    {
        if (movement != null)
        {
            movement.EndAttack();
        }
        isLight = false;
    }
}

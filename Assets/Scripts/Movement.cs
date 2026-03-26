using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
            moveAction.action.Enable();
    }

    private void OnDisable()
    {
            moveAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();

        float horizontal = moveInput.x;

        
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, 0f);
    }
}


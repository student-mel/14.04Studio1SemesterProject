using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isLight; //to check if light attack has been input

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    void Awake()
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
        if (!isLight)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }

        float horizontal = moveInput.x;

        bool isWalkingForward = !isLight && horizontal > 0.1f;
        bool isWalkingBackward = !isLight && horizontal < -0.1f;

        animator.SetBool("isWalkingForward", isWalkingForward);
        animator.SetBool("isWalkingBackward", isWalkingBackward);
    }


    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, 0f);
    }

    public void StartLightAttack()
    {
        if (isLight) return;

        isLight = true;

        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        animator.ResetTrigger("isLight");
        animator.SetTrigger("isLight");

        hitbox.SetActive(true);
    }

    public void EndAttack()
    {
        isLight = false;
        hitbox.SetActive(false);
    }

}


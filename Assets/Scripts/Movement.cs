using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Movement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private Animator animator;
    [SerializeField] private float hurtDuration = 0.7f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isLight; //to check if light attack has been input
    private bool isHurt;

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
        if (!isLight && !isHurt)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
        }
        else
        {
            moveInput = Vector2.zero;
        }

        float horizontal = moveInput.x;

        bool isWalkingForward = !isLight && !isHurt && horizontal > 0.1f;
        bool isWalkingBackward = !isLight && !isHurt && horizontal < -0.1f;

        animator.SetBool("isWalkingForward", isWalkingForward);
        animator.SetBool("isWalkingBackward", isWalkingBackward);
    }


    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, 0f);
    }

    public void StartLightAttack()
    {
        if (isLight || isHurt) return;

        isLight = true;

        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        animator.ResetTrigger("isLight");
        animator.SetTrigger("isLight");

        //hitbox.SetActive(true);
    }

    public void EndAttack()
    {
        isLight = false;
        //hitbox.SetActive(false);
    }

    public void StartHurt()
    {
        if (isHurt) return;

        isHurt = true;
        isLight = false;

        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        hitbox.SetActive(false);

        StopCoroutine("EndHurtAfterTime");
        StartCoroutine("EndHurtAfterTime");
    }

    private IEnumerator EndHurtAfterTime()
    {
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }

}


using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Linq;

public class Movement : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private GameObject hitbox;
    [SerializeField] private GameObject heavyHitbox;
    [SerializeField] private Transform hitboxTransform;
    [SerializeField] private float hitboxOffsetX = 0.8f;
    [SerializeField] private Animator animator;
    [SerializeField] private float hurtDuration = 0.7f;

    [SerializeField] private float lightStartupTime = 0.25f;
    [SerializeField] private float lightActiveTime = 0.08f;
    [SerializeField] private float heavyStartupTime = 0.65f;
    [SerializeField] private float heavyActiveTime = 0.12f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isLight; //to check if light attack has been input
    private bool isHeavy; //to check if heavy attack has been input
    private bool isHurt;

    public PlayerInputHandler inputHandler { get; private set; }
    [Range(1, 2)] public int PlayerIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LateStart());
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();

        if (inputHandler == null)
        {
            PlayerInputHandler[] players = FindObjectsByType<PlayerInputHandler>(FindObjectsSortMode.None);
            inputHandler = players.FirstOrDefault(p => p.PlayerIndex == PlayerIndex);

            inputHandler.MoveEvent += OnMove;
        }
    }

    private void OnEnable()
    {
        moveAction.action.Enable();

        if (inputHandler == null)
        {
            PlayerInputHandler[] players = FindObjectsByType<PlayerInputHandler>(FindObjectsSortMode.None);
            inputHandler = players.FirstOrDefault(p => p.PlayerIndex == PlayerIndex);

            if (inputHandler == null) return;
            inputHandler.MoveEvent += OnMove;
        }
        else
        {
            inputHandler.MoveEvent += OnMove;
        }
    }

    private void OnDisable()
    {
        moveAction.action.Disable();

        inputHandler.MoveEvent -= OnMove;
    }

    void OnMove(Vector2 _input)
    {
        if (!isLight && !isHeavy && !isHurt)
        {
            moveInput = _input;
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {

        float horizontal = moveInput.x;

        bool isWalkingForward = !isLight && !isHeavy && !isHurt && horizontal > 0.1f;
        bool isWalkingBackward = !isLight && !isHeavy && !isHurt && horizontal < -0.1f;

        animator.SetBool("isWalkingForward", isWalkingForward);
        animator.SetBool("isWalkingBackward", isWalkingBackward);

        if (hitboxTransform != null)
        {
            Vector3 pos = hitboxTransform.localPosition;

            if (horizontal > 0.1f)
                pos.x = hitboxOffsetX;
            else if (horizontal < -0.1f)
                pos.x = -hitboxOffsetX;

            hitboxTransform.localPosition = pos;
        }
    }


    void FixedUpdate()
    {
        Vector2 targetPosition = rb.position + new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime, 0f);
        rb.MovePosition(targetPosition);
    }

    public void StartLightAttack()
    {
        if (isLight || isHeavy || isHurt) return;

        isLight = true;

        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        animator.ResetTrigger("isLight");
        animator.SetTrigger("isLight");

        StartCoroutine(LightAttackHitTiming());
    }

    private IEnumerator LightAttackHitTiming()
    {
        if (hitbox != null)
            hitbox.SetActive(false);

        yield return new WaitForSeconds(lightStartupTime);

        if (hitbox != null)
            hitbox.SetActive(true);

        yield return new WaitForSeconds(lightActiveTime);

        if (hitbox != null)
            hitbox.SetActive(false);
    }

    public void EndAttack()
    {
        isLight = false;
        hitbox.SetActive(false);
    }

    public void StartHurt()
    {
        if (isHurt) return;

        isHurt = true;
        isLight = false;
        isHeavy = false;

        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;

        StopAllCoroutines();

        heavyHitbox.SetActive(false);
        hitbox.SetActive(false);

        StopCoroutine("EndHurtAfterTime");
        StartCoroutine("EndHurtAfterTime");
    }

    private IEnumerator EndHurtAfterTime()
    {
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }

    public void StartHeavyAttack()
    {
        if (isLight || isHeavy || isHurt) return;

        isHeavy = true;
        moveInput = Vector2.zero;

        animator.ResetTrigger("isHeavy");
        animator.SetTrigger("isHeavy");

        StartCoroutine(HeavyAttackHitTiming());
    }

    private IEnumerator HeavyAttackHitTiming()
    {
        if (heavyHitbox != null)
            heavyHitbox.SetActive(false);

        yield return new WaitForSeconds(heavyStartupTime);

        if (heavyHitbox != null)
            heavyHitbox.SetActive(true);

        yield return new WaitForSeconds(heavyActiveTime);

        if (heavyHitbox != null)
            heavyHitbox.SetActive(false);
    }

    public void EndHeavyAttack()
    {
        isHeavy = false;
        heavyHitbox.SetActive(false);
    }
}


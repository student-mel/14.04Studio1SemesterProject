using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class AnimationScript : MonoBehaviour
{
    [SerializeField] private InputActionReference attackAction;
    [SerializeField] private InputActionReference heavyAttackAction;
    [SerializeField] private Animator animator;
    [SerializeField] private Movement movement;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isLight;

    public PlayerInputHandler inputHandler { get; private set; }
    [Range(1, 2)] public int PlayerIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;

        if (inputHandler == null)
        {
            PlayerInputHandler[] players = FindObjectsByType<PlayerInputHandler>(FindObjectsSortMode.None);
            inputHandler = players.FirstOrDefault(p => p.PlayerIndex == PlayerIndex);
            
        }
    }

    private void OnEnable()
    {
        if (inputHandler == null)
        {
            PlayerInputHandler[] players = FindObjectsByType<PlayerInputHandler>(FindObjectsSortMode.None);
            inputHandler = players.FirstOrDefault(p => p.PlayerIndex == PlayerIndex);

            if (inputHandler == null) return;
        }
    }

    private void OnDisable()
    {

    }

    private void OnAttack()
    {
        if (movement != null)
        {
            movement.StartLightAttack();
        }
    }

    private void OnHeavyAttack()
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

    public void EndHeavyAttack()
    {
        if (movement != null)
        {
            movement.EndHeavyAttack();
        }
    }
}

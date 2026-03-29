using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public UnityAction<Vector2> MoveEvent;
    public UnityAction MoveEndedEvent;
    public UnityAction AttackEvent;
    public UnityAction AttackEndedEvent;
    public UnityAction Attack2Event;
    public UnityAction Attack2EndedEvent;

    private PlayerInput input;
    private Vector2 moveInput = new Vector2();

    private bool attackedThisFrame = false;

    private InputDebug inputDisplay;

    private InputBuffer buffer;

    public int inputIndex { get; private set; } = 0;

    public bool debug = false;

    public int PlayerIndex { get; private set; }


    private void Start()
    {
        input = GetComponent<PlayerInput>();
        PlayerIndex = input.user.index + 1;

        InputDebug[] moveUIs = FindObjectsByType<InputDebug>(FindObjectsSortMode.None);
        inputDisplay = moveUIs.FirstOrDefault(m => m.Index == PlayerIndex);
        inputDisplay.AssignInputHandler(this);

        buffer = FindAnyObjectByType<InputBuffer>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            AttackEndedEvent?.Invoke();
            attackedThisFrame = false;
            return;
        }

        if (context.started)
        {
            EventBus.Emit("action", input.user.index);

            AttackEvent?.Invoke();
            attackedThisFrame = true;

            if (debug)
                Debug.Log($"Player {(input.user.index + 1)} Attacked");
        }
    }

    public void OnAttack2(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Attack2EndedEvent?.Invoke();
            attackedThisFrame = false;
            return;
        }

        if (context.started)
        {
            EventBus.Emit("action", input.user.index);

            Attack2Event?.Invoke();
            attackedThisFrame = true;

            if (debug)
                Debug.Log($"Player {(input.user.index + 1)} Heavy Attacked");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.started) inputIndex++;

        MoveEvent?.Invoke(moveInput);

        if(debug)
            Debug.Log($"Player {(input.user.index + 1)} Moving");

        if(context.canceled) MoveEndedEvent?.Invoke();
    }
    private void Update()
    {
        UpdateInput();
        buffer?.ClearExpiredInputs(Time.time);
    }

    public void UpdateInput()
    {
        if (!inputDisplay) return;

        if (attackedThisFrame)
        {
            if (moveInput.x > 0.1f)
            {
                inputDisplay.AddMoveToQueue(2, 1);
            }
            else if (moveInput.x < -0.1f)
            {
                inputDisplay.AddMoveToQueue(2, 0);
            }
            else
            {
                inputDisplay.AddMoveToQueue(2, null);
            }

            attackedThisFrame = false;

            buffer.AddAttackInput(PlayerIndex, InputType.Light);
        }
        else
        {
            if (moveInput.x > 0.1f)
                inputDisplay.AddMoveToQueue(null, 1);
            else if (moveInput.x < -0.1f)
                inputDisplay.AddMoveToQueue(null, 0);
        }
    }
}

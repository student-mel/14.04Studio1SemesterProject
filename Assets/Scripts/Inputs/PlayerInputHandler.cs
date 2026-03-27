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

    private PlayerInput input;
    private Vector2 moveInput = new Vector2();

    private bool attackedThisFrame = false;

    private MoveUI moveDisplay;

    private InputBuffer buffer;

    public int inputIndex { get; private set; } = 0;

    public bool debug = false;

    public int PlayerIndex { get; private set; }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        PlayerIndex = input.user.index + 1;

        MoveUI[] moveUIs = FindObjectsByType<MoveUI>(FindObjectsSortMode.None);
        moveDisplay = moveUIs.FirstOrDefault(m => m.Index == PlayerIndex);
        moveDisplay.AssignInputHandler(this);

        buffer = new InputBuffer();

        buffer.id = PlayerIndex;
        buffer.playerIntent = new CombatIntent();
        CombatResolver.i.SetInputBuffer(PlayerIndex, buffer);

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackEvent?.Invoke();

            attackedThisFrame = true;

            if (debug)
                Debug.Log($"Player {(input.user.index + 1)} Attacked");
        }

        if (context.canceled)
        {
            AttackEndedEvent?.Invoke();
            attackedThisFrame = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.started) inputIndex++;

        if(Mathf.Abs(moveInput.x) > 0.1f)
            MoveEvent?.Invoke(moveInput);

        if(debug)
            Debug.Log($"Player {(input.user.index + 1)} Moving");

        if(context.canceled) MoveEndedEvent?.Invoke();
    }
    private void Update()
    {
        UpdateInput();
        buffer.ClearExpiredInputs(Time.time);
    }

    public void UpdateInput()
    {
        if (attackedThisFrame)
        {
            if (moveInput.x > 0.1f)
                moveDisplay.AddMoveToQueue(2, 1);
            else if (moveInput.x < -0.1f)
                moveDisplay.AddMoveToQueue(2, 0);
            else
                moveDisplay.AddMoveToQueue(2, null);
            attackedThisFrame = false;

            buffer.AddInput(CombatActionType.LightAttack, Time.time);
        }
        else
        {
            if (moveInput.x > 0.1f)
                moveDisplay.AddMoveToQueue(null, 1);
            else if (moveInput.x < -0.1f)
                moveDisplay.AddMoveToQueue(null, 0);
        }
    }
}

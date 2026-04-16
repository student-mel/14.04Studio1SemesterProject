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

    public InputBuffer buffer {get; private set;}

    public int inputIndex { get; private set; } = 0;

    public bool debug = false;

    public int PlayerIndex { get; private set; }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        PlayerIndex = input.user.index + 1;

        // InputDebug[] moveUIs = FindObjectsByType<InputDebug>(FindObjectsSortMode.None);
        // inputDisplay = moveUIs.FirstOrDefault(m => m.Index == PlayerIndex);
        // inputDisplay.AssignInputHandler(this);

        buffer = GetComponent<InputBuffer>();
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        // if (context.canceled)
        // {
        //     AttackEndedEvent?.Invoke();
        //     attackedThisFrame = false;
        //     return;
        // }

        if (context.started)
        {
            buffer.AddInput(InputType.LightAtt);
            
            // EventBus.Emit("action", input.user.index);
            //
            // AttackEvent?.Invoke();
            // attackedThisFrame = true;
            //
            // if (debug)
            //     Debug.Log($"Player {(input.user.index + 1)} pressed Attack 1");
        }
    }

    public void OnMediumAttack(InputAction.CallbackContext context)
    {
        if (context.started)buffer.AddInput(InputType.MediumAtt);
    }
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        // if (context.canceled)
        // {
        //     EventBus.Emit("on_heavy_attack_released");
        //     
        //     Attack2EndedEvent?.Invoke();
        //     attackedThisFrame = false;
        //     return;
        // }

        if (context.started)
        {
            buffer.AddInput(InputType.HeavyAtt);
            
            // EventBus.Emit("action", input.user.index);
            //
            // EventBus.Emit("on_heavy_attack_pressed");
            //
            // Attack2Event?.Invoke();
            // attackedThisFrame = true;
            //
            // if (debug)
            //     Debug.Log($"Player {(input.user.index + 1)} pressed Attack 2");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (context.started) buffer.AddInputStart(moveInput);
        if (context.performed) buffer.AddInput(moveInput);
        
        // if (context.started)
        //     inputIndex++;
        //
        // EventBus.Emit("on_move_value_changed", moveInput);
        //
        // MoveEvent?.Invoke(moveInput);
        //
        // if(debug)
        //     Debug.Log($"Player {(input.user.index + 1)} Moving");
        //
        // if (context.canceled)
        // {
        //     MoveEndedEvent?.Invoke();
        //     
        //     EventBus.Emit("on_move_ended");
        // }
    }
    private void Update()
    {
        //UpdateInput();
        // buffer?.ClearExpiredInputs(Time.time);
    }

    public void UpdateInput()
    {
        //buffer.AddInput(moveInput);
        
        // if (!inputDisplay) return;
        //
        // if (attackedThisFrame)
        // {
        //     if (moveInput.x > 0.1f)
        //     {
        //         inputDisplay.AddMoveToQueue(2, 1);
        //     }
        //     else if (moveInput.x < -0.1f)
        //     {
        //         inputDisplay.AddMoveToQueue(2, 0);
        //     }
        //     else
        //     {
        //         inputDisplay.AddMoveToQueue(2, null);
        //     }
        //
        //     attackedThisFrame = false;
        // }
        // else
        // {
        //     if (moveInput.x > 0.1f)
        //         inputDisplay.AddMoveToQueue(null, 1);
        //     else if (moveInput.x < -0.1f)
        //         inputDisplay.AddMoveToQueue(null, 0);
        // }
    }
}
public enum InputType
{
    Left,
    LeftUp,
    LeftDown,
    Right,
    RightUp,
    RightDown,
    Up,
    Down,
    LightAtt,
    MediumAtt,
    HeavyAtt,
    None
}

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
    private Vector2 movementInput = new Vector2();

    private InputDebug inputDisplay;

    public InputBuffer buffer {get; private set;}

    public int inputIndex { get; private set; } = 0;

    public bool debug = false;

    public int PlayerIndex { get; private set; }
    
    private bool moving = false;
    private bool movementStartedThisFrame = false;

    private bool attackingLight = false;
    private bool lightAttackStartedThisFrame = false;
    private bool attackingMedium = false;
    private bool mediumAttackStartedThisFrame = false;
    private bool attackingHeavy = false;
    private bool heavyAttackStartedThisFrame = false;

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        PlayerIndex = input.user.index + 1;
        
        buffer = GetComponent<InputBuffer>();
        buffer.handler = this;
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            buffer.AddInputStart(InputType.LightAtt);
            // attackingLight = true;
            // lightAttackStartedThisFrame = true;
        }

        if (context.canceled)
        {
            attackingLight = false;
            CancelAttack(InputType.LightAtt);
        }
    }

    public void OnMediumAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            buffer.AddInputStart(InputType.MediumAtt);
            // attackingMedium = true;
            // mediumAttackStartedThisFrame = true;
        }

        if (context.canceled)
        {
            attackingMedium = false;
            CancelAttack(InputType.MediumAtt);
        }
    }
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            buffer.AddInputStart(InputType.HeavyAtt);
            // attackingHeavy = true;
            // heavyAttackStartedThisFrame = true;
        }

        if (context.canceled)
        {
            attackingHeavy = false;
            CancelAttack(InputType.HeavyAtt);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

        if (context.started)
        {
            buffer.AddInputStart(movementInput);
            movementStartedThisFrame = true;
            moving = true;
        }

        if (context.canceled)
        {
            moving = false;
            CancelMovement();
        }
    }

    private void CancelAttack(InputType input)
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_attackinput_cancelled", input);
                break;
            case 2:
                EventBus.Emit("p2_attackinput_cancelled", input);
                break;
            default:
                break;
        }
    }

    private void CancelMovement()
    {
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_moveinput_cancelled");
                break;
            case 2:
                EventBus.Emit("p2_moveinput_cancelled");
                break;
            default:
                break;
        }
    }
    
    private void Update()
    {
        UpdateInput();
    }

    public void UpdateInput()
    {
        if (moving && !movementStartedThisFrame)
            buffer.AddInput(movementInput);
        else
            movementStartedThisFrame = false;
        
        // if(attackingLight && !lightAttackStartedThisFrame)
        //     buffer.AddInput(InputType.LightAtt);
        // else
        //     lightAttackStartedThisFrame = false;
        //
        // if(attackingMedium && !mediumAttackStartedThisFrame)
        //     buffer.AddInput(InputType.MediumAtt);
        // else
        //     mediumAttackStartedThisFrame = false;
        //
        // if(attackingHeavy && !heavyAttackStartedThisFrame)
        //     buffer.AddInput(InputType.HeavyAtt);
        // else
        //     heavyAttackStartedThisFrame = false;
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
    SpecialAtt,
    None
}

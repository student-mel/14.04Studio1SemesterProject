using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput input;
    private Vector2 movementInput = new Vector2();

    private InputDebug inputDisplay;

    public InputBuffer buffer {get; private set;}

    public int inputIndex { get; private set; } = 0;

    public bool debug = false;

    public int PlayerIndex { get; private set; }
    
    private bool moving = false;
    private bool movementStartedThisFrame = false;

    public void Init()
    {
        input = GetComponent<PlayerInput>();
        PlayerIndex = input.user.index + 1;
        
        buffer = GetComponent<InputBuffer>();
        buffer.handler = this;
        //Debug.Log(buffer);
    }

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            buffer.AddInputStart(InputType.LightAtt);
        }

        if (context.canceled)
        {
            CancelAttack(InputType.LightAtt);
        }
    }

    public void OnMediumAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            buffer.AddInputStart(InputType.MediumAtt);
        }

        if (context.canceled)
        {
            CancelAttack(InputType.MediumAtt);
        }
    }
    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            buffer.AddInputStart(InputType.HeavyAtt);
        }

        if (context.canceled)
        {
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
        buffer.StopMovement();
        switch (PlayerIndex)
        {
            case 1:
                EventBus.Emit("p1_dirinput_cancelled");
                break;
            case 2:
                EventBus.Emit("p2_dirinput_cancelled");
                break;
            default:
                break;
        }
    }
    
    private void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (moving && !movementStartedThisFrame)
            buffer.AddInput(movementInput);
        else
            movementStartedThisFrame = false;
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

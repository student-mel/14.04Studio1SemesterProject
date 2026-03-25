using System;
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

    public bool debug = false;

    public int PlayerIndex => input.user.index + 1;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackEvent?.Invoke();

            if (debug)
                Debug.Log($"Player {(input.user.index + 1)} Attacked");
        }


        if(context.canceled) AttackEndedEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());

        if(debug)
            Debug.Log($"Player {(input.user.index + 1)} Moving");

        if(context.canceled) MoveEndedEvent?.Invoke();
    }
}

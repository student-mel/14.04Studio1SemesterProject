using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public UnityAction<Vector2> MoveEvent;
    public UnityAction AttackEvent;

    private PlayerInput input;

    public bool debug = false;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        AttackEvent?.Invoke();

        if(debug)
            Debug.Log($"Player {(input.user.index + 1)} Attacked");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());

        if(debug)
            Debug.Log($"Player {(input.user.index + 1)} Moving");
    }
}

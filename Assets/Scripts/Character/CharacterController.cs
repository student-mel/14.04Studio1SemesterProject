using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    bool isGrounded = true;
    bool isJumping = false;
    
    private Vector2 moveVector;
    private Vector3 relativeDir;
    public string[] actionStrs;
    
    Rigidbody rigidbody;
    Animator animator;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();        
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe("on_move", OnMove);
        EventBus.Subscribe("on_attack" , OnAttack);
    }

    private void OnDisable()
    {
        EventBus.Subscribe("on_move", OnMove);
        EventBus.Subscribe("on_attack" , OnAttack);
    }

    private void Start()
    {
        // set relative dir
    }

    private void Update()
    {
        if (Keyboard.current.aKey.isPressed)
        {
            moveVector.x = -1;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            moveVector.x = 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            moveVector.y = -1;
        }
        if (Keyboard.current.wKey.isPressed)
        {
            moveVector.y = 1;
        }

        if (moveVector.magnitude > 0)
        {
            OnMove(moveVector);
        }
    }

    private void OnMove(object obj)
    {
        moveVector = (Vector2)obj;
        if (!isGrounded) return;
        if (moveVector.y > 0)
            Jump();
        else if (moveVector.y == 0)
            Walk();
        else
            Crouch();
    }
    
    private void OnAttack(object obj)
    {
        
    }

    void Crouch()
    {
    }

    void Jump()
    {
        /*if (isJumping) return;
        isJumping = true;*/
        rigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
    }

    void Walk()
    {
        
    }
}

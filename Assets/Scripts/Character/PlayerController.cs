using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour, IDamageable, IMoveable
    {
        [field: SerializeField] public float MaxHealth { get; } = 100f;
        public float CurrentHealth { get; set; }
        
        bool isGrounded = true;
        bool isJumping = false;
        
        private Vector2 moveVector;
        private Vector3 relativeDir;
        public string[] actionStrs;
        
        Animator animator;
        public Rigidbody RB { get; set; }
        public bool IsFacingRight { get; set; }
        
        public PlayerStateMachine StateMachine {get; set;}

        public enum AnimationTriggerType
        {
            
        }

        private void Awake()
        {
            RB = GetComponent<Rigidbody>();        
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
            InitialisePlayer();
        }
        
        void InitialisePlayer()
        {
            CurrentHealth = MaxHealth;
            // set relative dir 
            CheckRelativeDir();
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
        
        public void CheckRelativeDir()
        {
        }

        public void OnMove(object obj)
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
            RB.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }

        void Walk()
        {
            
        }

        public void TakeDamage(float dmg)
        {
            CurrentHealth -= dmg;
            if (CurrentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
        }
     }
}

using System;
using UnityEngine;
using Character.PlayerHFSM;

namespace Character
{
    public class PlayerController : MonoBehaviour, IDamageable, IMoveable
    {
        [Header("Player")] public PlayerEnum player;
        public enum PlayerEnum
        {
            PlayerOne,
            PlayerTwo
        };
        [Header("Health")]
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; set; }
        
        //bool isGrounded = true;
        [SerializeField] private string nextMove = "Null";
        
        public enum MoveEnum {Null, MoveLeft, MoveRight,Crouch}

        public MoveEnum moveenum;
        private string moveName = "Null";
        public string MoveName => moveName;
        
        public string nextAttack = "Null";
        private string attackName = "Null";
        public string AttackName => attackName;
        
        public string nextReaction = "Null";
        private string reactionName = "Null";
        public string ReactionName => reactionName;
        
        public Animator animator;

        [field: SerializeField]public float MoveSpeed { get; set; } = 1.5f;
        [field: SerializeField] public float JumpForce { get; set; } = 25f;
        
        public Vector3 RelativeDir {get; private set;}
        public Rigidbody RB { get; private set; }
        [SerializeField] private Transform opponent;
        public bool canFlip = true;
        public bool IsFacingRight { get; set; }
        
        [Header("Player HFSM")]
        public PlayerStateMachine StateMachine {get; set;}

        private PlayerState groundedState, airborneState, stunState, attackState;
        public PlayerState GroundedState => groundedState;
        public PlayerState AirborneState => airborneState;
        public PlayerState StunState => stunState;
        public PlayerState AttackState => attackState;
        
        public enum AnimationTriggerType
        {
            
        }

        private void OnValidate()
        {
            nextMove = moveenum.ToString();
        }

        private void Awake()
        {
            RB = GetComponent<Rigidbody>();   
            animator = transform.GetChild(0).GetComponentInChildren<Animator>();
            InitialiseStateMachine();
        }

        void InitialiseStateMachine()
        {
            StateMachine = new PlayerStateMachine();
            
            groundedState = new StateGrounded(this, StateMachine);
            airborneState = new StateAirborne(this, StateMachine);
            stunState = new StateStun(this, StateMachine);
            attackState = new StateAttack(this, StateMachine);
            
            StateMachine.Initialise(groundedState);
            
            IsFacingRight = opponent.position.x > transform.position.x;
            Flip(false);
        }

        void SubscribeInputEvents()
        {
            string p = player ==  PlayerEnum.PlayerOne ? "p1_" : "p2_";
            
            EventBus.Subscribe($"{p}move", OnMove);
            EventBus.Subscribe($"{p}moveinput_cancelled" , OnMoveCancelled);
            EventBus.Subscribe($"{p}attack", OnAttack);
            EventBus.Subscribe($"{p}hurt", OnHurt);
        }

        void UnsubscribeInputEvents()
        {
            string p = player ==  PlayerEnum.PlayerOne ? "p1_" : "p2_";
            EventBus.Unsubscribe($"{p}move", OnMove);
            EventBus.Unsubscribe($"{p}moveinput_cancelled" , OnMoveCancelled);
            EventBus.Unsubscribe($"{p}attack", OnAttack);
            EventBus.Unsubscribe($"{p}hurt", OnHurt);
        }
  
        private void OnEnable()
        {
            SubscribeInputEvents();
        }

        private void OnDisable()
        {
            UnsubscribeInputEvents();
        }

        private void Start()
        {
            InitialisePlayer();
        }
        
        void InitialisePlayer()
        {
            CurrentHealth = MaxHealth;
            CheckRelativeDir();
        }

        private void Update()
        {
            CheckRelativeDir();
            moveName = nextMove;
            attackName = nextAttack;
            StateMachine.CurrentState?.UpdateState();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState?.FixedUpdateState();
        }

        public void CheckRelativeDir()
        {
            if (opponent == null) return;

            bool shouldFaceRight = opponent.position.x > transform.position.x;

            if (shouldFaceRight != IsFacingRight)
            {
                Flip();
            }
        }
        
        void Flip(bool isFlipping = true)
        {
            if (!canFlip) return;
            
            if (isFlipping)
                IsFacingRight = !IsFacingRight;

            Quaternion rot = Quaternion.Euler(0, 90 * (IsFacingRight? 1 : -1), 0);
            RelativeDir = IsFacingRight ? Vector3.right : -Vector3.right;
            transform.GetChild(0).localRotation = rot;
        }

        public void OnMove(object obj)
        {
            CharacterMove move = obj as CharacterMove;
            nextMove = move?.Name;
            
            //Debug.LogWarning("Moving");
        }
              
        private void OnMoveCancelled(object obj)
        {
            nextMove = "Null";
            //Debug.LogWarning("Cancelled");
        }
        
        private void OnAttack(object obj)
        {
            //Debug.LogWarning("OnAttack");
            CharacterMove move = obj as CharacterMove;
            if (StateMachine.CurrentState == attackState)
                return;
            nextAttack = move?.Name;
        }
        
        private void OnHurt(object obj)
        {
            nextReaction = (string) obj;
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

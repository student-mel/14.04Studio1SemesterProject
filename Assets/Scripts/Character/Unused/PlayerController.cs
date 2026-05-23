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
        
        [field: SerializeField, Header("Health")] public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; private set; }
        
        //bool isGrounded = true;
        private string nextMove = "Null";
        
        public enum MoveEnum {Null, MoveLeft, MoveRight,Crouch}
        
        [Header("Move Input Debug")] 
        public MoveEnum moveenum;
        private string moveName = "Null";
        public string MoveName => moveName;
        [Header("Attack Input")] 
        public string nextAttack = "Null";
        private string attackName = "Null";
        public string AttackName => attackName;
        [Header("Reaction Input")] 
        public string nextReaction = "Null";
        private string reactionName = "Null";
        public string ReactionName => reactionName;
        [Header("Character Animator")]
        public Animator animator;
        public Animator hitboxDebugAnimator;

        [field: SerializeField, Header("Movement Settings")]public float MoveSpeed { get; set; } = 1.5f;
        [field: SerializeField] public float JumpForce { get; set; } = 25f;
        public float ForwardMultiplier { get; }
        public float BackwardMultiplier { get; }
        [HideInInspector] public bool CanFlip { get; set; } = true;
        public bool IsFacingRight { get; set; }
        public bool IsGrounded { get; }
        public Vector3 RelativeDir {get; private set;}
        private Vector3 spawnPosition;
        public Vector2 MoveDir {get; private set;}
        
        public Rigidbody RB { get; private set; }
        [SerializeField, Header("Opponent")] public PlayerController opponent;
        
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
        
        [Header("Results")] public string rhythmResults;

        private void OnValidate()
        {
            nextMove = moveenum.ToString();
        }

        private void Awake()
        {
            RB = GetComponent<Rigidbody>();   
            animator = transform.GetChild(0).GetComponentInChildren<Animator>();
        }

        void InitialiseStateMachine()
        {
            StateMachine = new PlayerStateMachine();
            
            groundedState = new StateGrounded(this, StateMachine);
            airborneState = new StateAirborne(this, StateMachine);
            stunState = new StateStun(this, StateMachine);
            attackState = new StateAttack(this, StateMachine);
            
            StateMachine.Initialise(groundedState);
            
            IsFacingRight = opponent.transform.position.x > transform.position.x;
            Flip(false);
        }

        void SubscribeInputEvents()
        {
            string p = player ==  PlayerEnum.PlayerOne ? "p1_" : "p2_";
            
            EventBus.Subscribe($"{p}dirinput_vector", OnMove);
            EventBus.Subscribe($"{p}dirinput_cancelled" , OnMoveCancelled);
            EventBus.Subscribe($"{p}attack", OnAttack);
            EventBus.Subscribe($"{p}hurt", OnHurt);
            EventBus.Subscribe("actionResult", GetActionResult);

            void GetActionResult(object obj)
            {
                PlayerResult result = (PlayerResult)obj;
                if (result.Index != (int)player) return;

                this.rhythmResults = result.Result;
            }
        }

        void UnsubscribeInputEvents()
        {
            string p = player ==  PlayerEnum.PlayerOne ? "p1_" : "p2_";
            EventBus.Unsubscribe($"{p}dirinput_vector", OnMove);
            EventBus.Unsubscribe($"{p}dirinput_cancelled" , OnMoveCancelled);
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
            InitialiseStateMachine();
            
            spawnPosition = transform.position;
            InitialisePlayer();
            EventBus.Emit("set_maxhealth", MaxHealth);
        }
        
        public void InitialisePlayer()
        {
            CurrentHealth = MaxHealth;
            transform.position = spawnPosition;
            CheckRelativeDir();
            StateMachine.Initialise(groundedState);
            
            IsFacingRight = opponent.transform.position.x > transform.position.x;
            Flip(false);
            EventBus.Emit($"p{(int)player+1}_set_currenthealth", CurrentHealth);
        }

        private void Update()
        {
            CheckRelativeDir();
            moveName = nextMove;
            //attackName = nextAttack;
            //reactionName = nextReaction;
            StateMachine.CurrentState?.UpdateState();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState?.FixedUpdateState();
        }

        public void CheckRelativeDir()
        {
            if (opponent == null) return;

            bool shouldFaceRight = opponent.transform.position.x > transform.position.x;

            if (shouldFaceRight != IsFacingRight)
            {
                Flip();
            }
        }
        
        void Flip(bool isFlipping = true)
        {
            if (!CanFlip) return;
            
            if (isFlipping)
                IsFacingRight = !IsFacingRight;

            Quaternion rot = Quaternion.Euler(0, 90 * (IsFacingRight? 1 : -1), 0);
            RelativeDir = IsFacingRight ? Vector3.right : -Vector3.right;
            transform.GetChild(0).localRotation = rot;
        }

        public void OnMove(object obj)
        {
            if (GameManager.InputLocked) return;
            MoveDir = (Vector2)obj;

            // tutorial movement event
            if (MoveDir.x > 0.1f)
                EventBus.Emit("player_move_right", player);

            else if (MoveDir.x < -0.1f)
                EventBus.Emit("player_move_left", player);

            return;
            //Moveset move = obj as Moveset;
            //nextMove = move?.Name;

            //Debug.LogWarning("Moving");
        }
              
        private void OnMoveCancelled(object obj)
        {
            nextMove = "Null";
            //Debug.LogWarning("Cancelled");
        }
        
        private void OnAttack(object obj)
        {
            if (GameManager.InputLocked) return;
            //Debug.LogWarning("OnAttack");
            Moveset move = obj as Moveset;
            if (StateMachine.CurrentState == attackState)
                return;
            if (!StateMachine.CurrentSubState.canAttack)
                return;
            attackName = move?.Name;

            // tutorial emit
            EventBus.Emit("player_attack", attackName);

            StateMachine.ChangeState(AttackState);
        }
        
        private void OnHurt(object obj)
        {
            reactionName = (string) obj;
            StateMachine.ChangeState(StunState);
        }

        public void TakeDamage(float dmg)
        {
            int otherPlayer = player ==  PlayerEnum.PlayerOne ? 1 : 0;
            string oppResult = opponent.rhythmResults;
            EventBus.Emit("hit_result", new PlayerResult(otherPlayer, oppResult, true));
            
            CurrentHealth -= dmg * GetDamageMult(oppResult);
            AudioManager.Instance?.PlayHit(gameObject);

            EventBus.Emit($"p{(int)player+1}_set_currenthealth", CurrentHealth);
            if (CurrentHealth <= 0f)
            {
                Die();
            }
        }

        public void Die()
        {
            animator.SetTrigger("die");
        }
        
        float GetDamageMult(string result)
        {
            switch (result)
            {
                case "Perfect":
                    return 1.75f;
                /*case "Great":
                    return 1.3f;*/  
                /*case "Good":
                    return 1.15f;*/
                case "Syncopated":
                    return 2f;
                case "Miss":
                    return 0.5f;
            }
        
            return 1f;
        }
     }
}

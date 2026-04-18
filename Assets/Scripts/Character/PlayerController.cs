using UnityEngine;
using Character.PlayerHFSM;

namespace Character
{
    public class PlayerController : MonoBehaviour, IDamageable, IMoveable
    {
        [field: SerializeField] public float MaxHealth { get; } = 100f;
        public float CurrentHealth { get; set; }
        
        bool isGrounded = true;
        bool isJumping = false;
        
        public Vector2 MoveInput { get; private set; }
        private Vector3 relativeDir;
        public string[] actionStrs;
        
        Animator animator;
        public Rigidbody RB { get; set; }
        public bool IsFacingRight { get; set; }
        
        [Header("Player HFSM")]
        public PlayerStateMachine StateMachine {get; set;}

        private PlayerState groundedState, airborneState, stunState;
        public PlayerState GroundedState => groundedState;
        public PlayerState AirborneState => airborneState;
        public PlayerState StunState => stunState;
        
        public enum AnimationTriggerType
        {
            
        }

        private void Awake()
        {
            RB = GetComponent<Rigidbody>();        
            animator = GetComponent<Animator>();
            InitialiseStateMachine();
        }

        void InitialiseStateMachine()
        {
            StateMachine = new PlayerStateMachine();
            
            groundedState = new StateGrounded(this, StateMachine);
            airborneState = new StateAirborne(this, StateMachine);
            stunState = new StateStun(this, StateMachine);
            
            StateMachine.Initialise(groundedState);
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
            CheckRelativeDir();
        }

        private void Update()
        {
            StateMachine.CurrentState?.UpdateState();
        }

        public void CheckRelativeDir()
        {
        }

        public void OnMove(object obj)
        {
            MoveInput = (Vector2)obj;
            /*if (!isGrounded) return;
            if (MoveInput.y > 0)
                Jump();
            else if (moveVector.y == 0)
                Walk();
            else
                Crouch();*/
        }
        
        private void OnAttack(object obj)
        {
            
        }

        
        void Jump()
        {
            /*/*if (isJumping) return;
            isJumping = true;#1#
            RB.AddForce(Vector3.up * 10f, ForceMode.Impulse);*/
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

using UnityEngine;
using Character.PlayerHFSM;

namespace Character
{
    public class PlayerController : MonoBehaviour, IDamageable, IMoveable
    {
        [Header("Health")]
        [field: SerializeField] public float MaxHealth { get; set; } = 100f;
        public float CurrentHealth { get; set; }
        
        public Vector2 MoveInput { get; private set; }
        bool isGrounded = true;
        public string[] actionStrs;
        
        Animator animator;
        
        private Vector3 relativeDir;
        public Rigidbody RB { get; set; }
        [SerializeField] private Transform opponent;
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
            animator = GetComponentInChildren<Animator>();
            InitialiseStateMachine();
        }

        void InitialiseStateMachine()
        {
            StateMachine = new PlayerStateMachine();
            
            groundedState = new StateGrounded(this, StateMachine);
            airborneState = new StateAirborne(this, StateMachine);
            stunState = new StateStun(this, StateMachine);
            
            StateMachine.Initialise(groundedState);
            
            IsFacingRight = opponent.position.x > transform.position.x;
            Flip(false);
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
            CheckRelativeDir();
            StateMachine.CurrentState?.UpdateState();
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
            if (isFlipping)
                IsFacingRight = !IsFacingRight;

            Quaternion rot = Quaternion.Euler(0, 90 * (IsFacingRight? 1 : -1), 0);
            transform.GetChild(0).localRotation = rot;
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

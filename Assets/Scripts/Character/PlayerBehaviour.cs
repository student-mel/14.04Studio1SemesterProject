using System;
using RPGCharacterAnims.Actions;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour, IDamageable, IMoveable
{
    [Header("References")]
    public PlayerEnum player;
    public enum PlayerEnum
    {
        PlayerOne,
        PlayerTwo
    };
    public Animator animator;
    public CapsuleCollider playerColl;
    [SerializeField, Header("Opponent")] 
    public PlayerBehaviour opponent;

    #region  IMoveable Block
    public Rigidbody RB { get; private set; }

    public bool IsFacingRight { get; set; }
    [HideInInspector] public bool CanFlip { get; set; }  = true;
    
    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float checkDist = 0.2f;
    public bool IsGrounded => Physics.Raycast(transform.position, Vector3.down, checkDist, groundMask);
    public bool IsRising => RB.linearVelocity.y > 0f;
    public bool IsFalling => RB.linearVelocity.y < 0f;
    public bool IsCrouching => MoveDir.y < 0f;
    
    public Vector3 RelativeDir { get; private set; }
    public Vector2 MoveDir { get; set; } =  Vector2.zero;
    
    [field: SerializeField, Header("Movement Settings")]public float MoveSpeed { get; set; } = 1.5f;
    [field: SerializeField] public float JumpForce { get; set; } = 25f;
    [field: SerializeField] public float ForwardMultiplier { get; } = 1f;
    [field: SerializeField] public float BackwardMultiplier { get; } = 0.7f;
    #endregion

    #region  IDamageable Block
    [field: SerializeField, Header("Health")] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; private set; }

    private bool _isBlocking = false;
    public bool IsBlocking
    {
        get  => _isBlocking;
        private set
        {
            _isBlocking = value;
            animator.SetBool(Block, value);
        }
    }

    private bool canBlock = false;

    #endregion

    [Header("Rhythm")] public string rhythmResults;
    
    private StateMachine FSM;
    private Vector3 spawnPosition;
    
    private static readonly int MoveForward = Animator.StringToHash("moveForward");
    private static readonly int MoveBackward = Animator.StringToHash("moveBackward");
    private static readonly int Block = Animator.StringToHash("block");

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        InitialiseStateMachine(new StateMachine());

        FSM.ChangeState<MovementState>();
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
        spawnPosition = transform.position;
        
        InitialisePlayer();
        EventBus.Emit("set_maxhealth", MaxHealth);
    }

    private void Update()
    {
        CheckRelativeDir();
        CheckBlock();
        FSM.Update();
    }

    private void FixedUpdate()
    {
        FSM.FixedUpdate();
    }

    #region Initialise Events
        void SubscribeInputEvents()
        {
            string p = player ==  PlayerEnum.PlayerOne ? "p1_" : "p2_";
                
            EventBus.Subscribe($"{p}dirinput_vector", OnMove);
            EventBus.Subscribe($"{p}dirinput_cancelled" , OnMoveCancelled);
            EventBus.Subscribe($"{p}attack", OnAttack);
            EventBus.Subscribe($"{p}hurt", OnHurt);
            EventBus.Subscribe("actionResult", GetActionResult);
            EventBus.Subscribe($"{p}anticipate", OnAnticipate);
            EventBus.Subscribe($"{p}anticipate_cancel", OnAnticipateCancel);
        }

        void UnsubscribeInputEvents()
        {
            string p = player ==  PlayerEnum.PlayerOne ? "p1_" : "p2_";
            
            EventBus.Unsubscribe($"{p}dirinput_vector", OnMove);
            EventBus.Unsubscribe($"{p}dirinput_cancelled" , OnMoveCancelled);
            EventBus.Unsubscribe($"{p}attack", OnAttack);
            EventBus.Unsubscribe($"{p}hurt", OnHurt);
            EventBus.Unsubscribe("actionResult", GetActionResult);
        }
    #endregion

    #region Initialise Player and StateMachine 
    public void InitialisePlayer()
    {
        CurrentHealth = MaxHealth;
        transform.position = spawnPosition;
        CheckRelativeDir();
            
        IsFacingRight = opponent.transform.position.x > transform.position.x;
        Flip(false);
        EventBus.Emit($"p{(int)player+1}_set_currenthealth", CurrentHealth);
    }

    void InitialiseStateMachine(StateMachine fsm)
    {
        FSM = fsm;  
        fsm.AddState(new MovementState(this, fsm));
        fsm.AddState(new AttackState(this, fsm));
        fsm.AddState(new StunState(this, fsm));
    }
    #endregion

    private void OnMove(object obj)
    {
        if (GameManager.InputLocked)
        {
            MoveDir = Vector2.zero;
            return;
        }
        MoveDir = (Vector2)obj;
    }

    private void OnMoveCancelled(object obj)
    {
        MoveDir = Vector2.zero;
    }
    
    private void OnHurt(object obj)
    {
        FSM.ChangeState<StunState>();
    }

    private void OnAttack(object obj)
    {
    }

    private void OnAnticipate(object obj)
    {
        if (canBlock)
            IsBlocking = true;
    }
    
    private void OnAnticipateCancel(object obj)
    {
        IsBlocking = false;
    }

    void GetActionResult(object obj)
    {
        PlayerResult result = (PlayerResult)obj;
        if (result.Index != (int)player) return;

        this.rhythmResults = result.Result;
    }
    
    /*public void ApplyHit(float stunDuration)
    {
        fsm.ChangeState<StunState>();
        (fsm.CurrentState as StunState)?.SetStun(stunDuration);
    }*/
    float GetDamageMult(string result)
    {
        switch (result)
        {
            case "Perfect":
                return 1.75f;
            case "Syncopated":
                return 2f;
            case "Miss":
    
                return 0.5f;
        }
        
        return 1f;
    }

    public void Idle()
    {
        
    }
    
    private void CheckBlock()
    {
        // if grounded and moving or crouching and moving backwards
        if (IsGrounded && MoveDir.y <= 0)
        {
            canBlock = MoveDir.x > 0 ^ IsFacingRight;
        }
        else
            canBlock = false;
    }
    
    public void TakeDamage(float dmg)
    {
        int otherPlayer = player ==  PlayerEnum.PlayerOne ? 1 : 0;
        string oppResult = opponent.rhythmResults;
        EventBus.Emit("hit_result", new PlayerResult(otherPlayer, oppResult, true));
        float totalDamage = dmg * GetDamageMult(oppResult);
        CurrentHealth -= IsBlocking ? totalDamage * 0.1f : totalDamage;
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
}

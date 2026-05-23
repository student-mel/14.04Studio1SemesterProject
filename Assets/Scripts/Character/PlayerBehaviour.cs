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
    [SerializeField, Header("Opponent")] public PlayerBehaviour opponent;

    #region  IMoveable Block
    public Rigidbody RB { get; private set; }

    public bool IsFacingRight { get; set; }
    [HideInInspector] public bool canFlip { get; set; }  = true;

    public Vector3 RelativeDir { get; }
    
    [field: SerializeField, Header("Movement Settings")]public float MoveSpeed { get; set; } = 1.5f;
    [field: SerializeField] public float JumpForce { get; set; } = 25f;
    
    public void CheckRelativeDir()
    {
    }
    #endregion

    #region  IDamageable Block
    [field: SerializeField, Header("Health")] public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; private set; }
    
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
    #endregion

    [Header("Rhythm")] public string rhythmResults;
    
    private StateMachine fsm;
    private Vector3 spawnPosition;

    private void Awake()
    {
        fsm = new StateMachine();
        RB = GetComponent<Rigidbody>();

        var movement = new MovementState(this, fsm);
        var attack = new AttackState(this, fsm);
        var stun = new StunState(this, fsm);

        fsm.AddState(movement);
        fsm.AddState(attack);
        fsm.AddState(stun);

        fsm.ChangeState<MovementState>();
    }

    private void Update()
    {
        fsm.Update();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    public void Move(Vector3 direction, float speed)
    {
        RB.MovePosition(RB.position + direction * speed * Time.fixedDeltaTime);
    }

    public void ApplyHit(float stunDuration)
    {
        fsm.ChangeState<StunState>();
        (fsm.CurrentState as StunState)?.SetStun(stunDuration);
    }

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
    
    public void Die()
    {
        animator.SetTrigger("die");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform ledgeCheck;
    [SerializeField] Transform groundCheck;

    [SerializeField] Transform playerCheck;

    public FiniteStateMachine stateMachine;
    public Data_Entity entityData;

    public int lastDamageDirection { get; private set; }
    public Animator animator { get; private set; }
    public AnimationToStateMachine animToStateMachine { get; private set; }
    public Core Core { get; private set; }

    Vector2 velocityWorkspace;

    float currentHealth;
    float currentStunResistance;
    float lastDamageTime;

    protected bool isStunned;
    protected bool isDead;

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        animToStateMachine = GetComponent<AnimationToStateMachine>();
        Core = GetComponentInChildren<Core>();
    }

    public virtual void Start()
    {
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        Core.LogicUpdate();

        stateMachine.currentState.LogicUpdate();

        animator.SetFloat("yVelocity", Core.Movement.RB.velocity.y);

        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    // public virtual void Damage(AttackDetails attackDetails)
    // {
    //     lastDamageTime = Time.time;

    //     currentHealth -= attackDetails.damageAmount;
    //     currentStunResistance -= attackDetails.stunDamageAmount;

    //     AudioPlayer.Instance.PlayBloodSound();
        
    //     Instantiate(entityData.hitParticles, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));

    //     DamageHop(entityData.damageHopSpeed);

    //     lastDamageDirection = attackDetails.position.x > transform.position.x ? -1 : 1; 

    //     if (currentStunResistance <= 0)
    //     {
    //         isStunned = true;
    //     }

    //     if (currentHealth <= 0)
    //     {
    //         isDead = true;
    //     }
    // }

    public virtual void DamageHop(float yVelocity)
    {
        velocityWorkspace.Set(Core.Movement.RB.velocity.x, yVelocity);
        Core.Movement.RB.velocity = velocityWorkspace;
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAggroDistance, entityData.playerMask);
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAggroDistance, entityData.playerMask);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.playerMask);
    }

    public virtual void OnDrawGizmos()
    {
        if (Core != null)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * Core.Movement.FacingDirection * entityData.wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));    

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), .2f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAggroDistance), .2f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAggroDistance), .2f);
        }
    }
}

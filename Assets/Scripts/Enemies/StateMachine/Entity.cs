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

    public int facingDirection { get; private set; }
    public int lastDamageDirection { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveObject { get; private set; }
    public AnimationToStateMachine animToStateMachine { get; private set; }

    Vector2 velocityWorkspace;

    float currentHealth;
    float currentStunResistance;
    float lastDamageTime;

    protected bool isStunned;
    protected bool isDead;

    public virtual void Start()
    {
        facingDirection = 1;

        aliveObject = transform.GetChild(0).gameObject;
        rb = aliveObject.GetComponent<Rigidbody2D>();
        animator = aliveObject.GetComponent<Animator>();
        animToStateMachine = aliveObject.GetComponent<AnimationToStateMachine>();
        currentHealth = entityData.maxHealth;
        currentStunResistance = entityData.stunResistance;

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();

        if (Time.time >= lastDamageTime + entityData.stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        velocityWorkspace.Set(facingDirection * velocity, rb.velocity.y);
        rb.velocity = velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveObject.transform.right, entityData.wallCheckDistance, entityData.groundMask);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.groundMask);
    }

    public virtual void ResetStunResistance()
    {
        isStunned = false;
        currentStunResistance = entityData.stunResistance;
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        lastDamageTime = Time.time;

        currentHealth -= attackDetails.damageAmount;
        currentStunResistance -= attackDetails.stunDamageAmount;

        AudioPlayer.Instance.PlayBloodSound();
        
        Instantiate(entityData.hitParticles, aliveObject.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));

        DamageHop(entityData.damageHopSpeed);

        lastDamageDirection = attackDetails.position.x > aliveObject.transform.position.x ? -1 : 1; 

        if (currentStunResistance <= 0)
        {
            isStunned = true;
        }

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }

    public virtual void DamageHop(float yVelocity)
    {
        velocityWorkspace.Set(rb.velocity.x, yVelocity);
        rb.velocity = velocityWorkspace;
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveObject.transform.Rotate(0, 180f, 0);
    }

    public virtual bool CheckPlayerInMinAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveObject.transform.right, entityData.minAggroDistance, entityData.playerMask);
    }

    public virtual bool CheckPlayerInMaxAggroRange()
    {
        return Physics2D.Raycast(playerCheck.position, aliveObject.transform.right, entityData.maxAggroDistance, entityData.playerMask);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, aliveObject.transform.right, entityData.closeRangeActionDistance, entityData.playerMask);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData.groundCheckRadius, entityData.groundMask);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));    

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), .2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAggroDistance), .2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAggroDistance), .2f);
    }
}

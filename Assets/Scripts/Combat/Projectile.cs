using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float gravity;
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform damagePos;
    [SerializeField] float damageRadius;
    [SerializeField] bool shouldDestroyOnGroundHit;

    WeaponAttackDetails attackDetails;
    Animator animator;
    Rigidbody2D rb;
    Vector2 knockbackAngle;
    float knockbackStrength;

    int direction;

    float speed;
    float travelDistance;
    float xStartPos;

    bool isGravityOn;
    bool hasHitGround;
    bool hasHitPlayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        xStartPos = transform.position.x;

        rb.gravityScale = 0f;
        rb.velocity = transform.right * speed;    

        isGravityOn = false;
    }

    void Update()
    {
        if (!hasHitGround)
        {
            // attackDetails.position = transform.position;

            if (isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }    
    }

    void FixedUpdate()
    {
        if (!hasHitGround)
        {
            Collider2D damageHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, playerMask);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePos.position, damageRadius, groundMask);

            if (damageHit && !hasHitPlayer)
            {
                hasHitPlayer = true;
                IDamageable damageable = damageHit.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.Damage(attackDetails.damageAmount, direction);
                }
                IKnockbackable knockbackable = damageHit.GetComponent<IKnockbackable>();
                if (knockbackable != null)
                {
                    knockbackable.Knockback(knockbackAngle, knockbackStrength, direction);
                }
                animator.SetBool("hitSomething", true);
                Destroy(gameObject, .5f);
            }

            if (groundHit && !hasHitGround)
            {
                hasHitGround = true;
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
                if (shouldDestroyOnGroundHit)
                {
                    animator.SetBool("hitSomething", true);
                    Destroy(gameObject, .5f);
                }
            }

            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }    
        }
    }

    public void FireProjectile(float speed, float travelDistance, float damage, int direction, Vector2 knockbackAngle, float knockbackStrength)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        this.direction = direction;
        this.knockbackAngle = knockbackAngle;
        this.knockbackStrength = knockbackStrength;
        attackDetails.damageAmount = damage;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(damagePos.position, damageRadius);    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck { get => groundCheck; private set => groundCheck = value; }
    public Transform WallCheck { get => wallCheck; private set => wallCheck = value; }
    public Transform LedgeCheck { get => ledgeCheck; private set => ledgeCheck = value; }
    public Transform CeilingCheck { get => ceilingCheck; private set => ceilingCheck = value; }
    public float GroundCheckRadius { get => groundCheckRadius; set => groundCheckRadius = value; }
    public LayerMask GroundMask { get => groundMask; set => groundMask = value; }
    public float WallCheckDistance { get => wallCheckDistance; set => wallCheckDistance = value; }

    [Header("Ground data")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;

    [Header("Wall data")]
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;

    [Header("Ledge / Ceiling data")]
    [SerializeField] Transform ledgeCheck;
    [SerializeField] Transform ceilingCheck;

    public bool Ground
    {
        get => Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundMask);
    }

    public bool Ceiling
    {
        get => Physics2D.OverlapCircle(ceilingCheck.position, GroundCheckRadius, groundMask);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, groundMask);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(wallCheck.position, Vector2.right * -core.Movement.FacingDirection, wallCheckDistance, groundMask);
    }

    public bool Ledge
    {
        get => Physics2D.Raycast(ledgeCheck.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, groundMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(ceilingCheck.position, groundCheckRadius);  
    }
}

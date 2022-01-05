using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck
    { 
        get
        {
            if (groundCheck)
            {
                return groundCheck;
            }

            Debug.LogError("No groundCheck on " + core.transform.parent.name);
            return null;
        }
         private set => groundCheck = value; 
    }
    public Transform WallCheck
    { 
        get
        {
            if (wallCheck)
            {
                return wallCheck;
            }

            Debug.LogError("No wallCheck on " + core.transform.parent.name);
            return null;
        }
         private set => wallCheck = value; 
    }
    public Transform LedgeCheckHorizontal
    { 
        get
        {
            if (ledgeCheckHorizontal)
            {
                return ledgeCheckHorizontal;
            }

            Debug.LogError("No ledgeCheckHorizontal on " + core.transform.parent.name);
            return null;
        }
         private set => ledgeCheckHorizontal = value; 
    }
    public Transform LedgeCheckVertical
    { 
        get
        {
            if (ledgeCheckVertical)
            {
                return ledgeCheckVertical;
            }

            Debug.LogError("No ledgeCheckVertical on " + core.transform.parent.name);
            return null;
        }
         private set => ledgeCheckVertical = value; 
    }
    public Transform CeilingCheck
    { 
        get
        {
            if (ceilingCheck)
            {
                return ceilingCheck;
            }

            Debug.LogError("No ceilingCheck on " + core.transform.parent.name);
            return null;
        }
         private set => ceilingCheck = value; 
    }
    public float GroundCheckRadius{ get => groundCheckRadius; set => groundCheckRadius = value; }
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
    [SerializeField] Transform ledgeCheckHorizontal;
    [SerializeField] Transform ledgeCheckVertical;
    [SerializeField] Transform ceilingCheck;

    public bool Ground
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, groundMask);
    }

    public bool Ceiling
    {
        get => Physics2D.OverlapCircle(CeilingCheck.position, GroundCheckRadius, groundMask);
    }

    public bool WallFront
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, groundMask);
    }

    public bool WallBack
    {
        get => Physics2D.Raycast(WallCheck.position, Vector2.right * -core.Movement.FacingDirection, wallCheckDistance, groundMask);
    }

    public bool LedgeHorizontal
    {
        get => Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * core.Movement.FacingDirection, wallCheckDistance, groundMask);
    }

    public bool LedgeVertical
    {
        get => Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, wallCheckDistance, groundMask);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D RB { get; private set; }
    public int FacingDirection { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }

    Vector2 velocityWorkspace;

    protected override void Awake()
    {
        base.Awake();

        RB = GetComponentInParent<Rigidbody2D>();
    }

    void Start()
    {
        FacingDirection = 1;
    }

    public void LogicUpdate()
    {
        CurrentVelocity = RB.velocity;
    }

    #region Set functions
    public void SetVelocityZero()
    {
        RB.velocity = Vector2.zero;
        CurrentVelocity = Vector2.zero;
    }

    public void SetVelocityX(float velocityX)
    {
        velocityWorkspace.Set(velocityX, CurrentVelocity.y);
        RB.velocity = velocityWorkspace;
        CurrentVelocity = velocityWorkspace;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        velocityWorkspace = direction * velocity;
        RB.velocity = velocityWorkspace;
        CurrentVelocity = velocityWorkspace;
    }

    public void SetVelocityY(float velocityY)
    {
        velocityWorkspace.Set(CurrentVelocity.x, velocityY);
        RB.velocity = velocityWorkspace;
        CurrentVelocity = velocityWorkspace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        RB.velocity = velocityWorkspace;
        CurrentVelocity = velocityWorkspace;
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDirection)
        {
            Flip();
        }
    }

    void Flip()
    {
        FacingDirection *= -1;
        RB.transform.Rotate(0, 180f, 0);
    }
    #endregion
}

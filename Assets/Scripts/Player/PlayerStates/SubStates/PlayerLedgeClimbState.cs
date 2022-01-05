using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    Vector2 detectedPosition;
    Vector2 cornerPosition;
    Vector2 startPosition;
    Vector2 stopPosition;
    Vector2 velocityWorkspace;

    bool isHanging;
    bool isClimbing;
    bool jumpInput;
    bool isTouchingCeiling;

    int xInput;
    int yInput;

    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        player.Anim.SetBool("ledgeClimb", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();

        isHanging = true;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityZero();
        player.transform.position = detectedPosition;
        cornerPosition = DetermineCornerPosition();

        startPosition.Set(cornerPosition.x - (core.Movement.FacingDirection * playerData.startOffset.x), cornerPosition.y - playerData.startOffset.y);
        stopPosition.Set(cornerPosition.x + (core.Movement.FacingDirection * playerData.stopOffset.x), cornerPosition.y + playerData.stopOffset.y);

        player.transform.position = startPosition;
    }

    public override void Exit()
    {
        base.Exit();

        isHanging = false;
        
        if (isClimbing)
        {
            player.transform.position = stopPosition;
            isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isAnimationFinished)
        {
            if (isTouchingCeiling)
            {
                stateMachine.ChangeState(player.CrouchIdleState);    
            }
            else
            {
                stateMachine.ChangeState(player.IdleState);
            }
        }
        else 
        {
            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            jumpInput = player.InputHandler.JumpInput;

            core.Movement.SetVelocityZero();
            player.transform.position = startPosition;

            if (xInput == core.Movement.FacingDirection && isHanging && !isClimbing)
            {
                CheckForSpace();
                isClimbing = true;
                player.Anim.SetBool("ledgeClimb", true);
            }
            else if (yInput == -1 && isHanging && !isClimbing)
            {
                stateMachine.ChangeState(player.AirState);
            }
            else if (jumpInput && !isClimbing)
            {
                player.WallJumpState.DetermineWallJumpDirection(true);
                stateMachine.ChangeState(player.WallJumpState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    void CheckForSpace()
    {
        isTouchingCeiling = Physics2D.Raycast(cornerPosition + (Vector2.up * .015f) + (Vector2.right * core.Movement.FacingDirection * 0.015f), Vector2.up, playerData.standColliderHeight, core.CollisionSenses.GroundMask);
    }

    Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(core.CollisionSenses.WallCheck.position, Vector2.right * core.Movement.FacingDirection, core.CollisionSenses.WallCheckDistance, core.CollisionSenses.GroundMask);
        float xDist = xHit.distance;
        velocityWorkspace.Set((xDist + .015f) * core.Movement.FacingDirection, 0);

        RaycastHit2D yHit = Physics2D.Raycast(core.CollisionSenses.LedgeCheckHorizontal.position + (Vector3)(velocityWorkspace), Vector2.down, core.CollisionSenses.LedgeCheckHorizontal.position.y - core.CollisionSenses.WallCheck.position.y + .015f, core.CollisionSenses.GroundMask);
        float yDist = yHit.distance;

        velocityWorkspace.Set(core.CollisionSenses.WallCheck.position.x + xDist * core.Movement.FacingDirection, core.CollisionSenses.LedgeCheckHorizontal.position.y - yDist);
        return velocityWorkspace;
    }

    public void SetDetectedPosition(Vector2 position) => detectedPosition = position;
}

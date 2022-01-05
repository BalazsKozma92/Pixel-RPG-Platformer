using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected Data_MoveState stateData;

    protected bool wallDetected;
    protected bool ledgeDetected;
    protected bool isPlayerInMinAggroRange;

    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        ledgeDetected = core.CollisionSenses.LedgeVertical;
        wallDetected = core.CollisionSenses.WallFront;
        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityX(stateData.movementSpeed * core.Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.SetVelocityX(stateData.movementSpeed * core.Movement.FacingDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

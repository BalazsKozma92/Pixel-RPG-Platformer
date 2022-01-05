using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected Data_ChargeState stateData;

    protected bool isPlayerInMinAggroRange;
    protected bool ledgeDetected;
    protected bool wallDetected;
    
    protected bool performCloseRangeAction;
    protected bool isChargeTimeOver;

    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_ChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
        performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        ledgeDetected = core.CollisionSenses.LedgeVertical;
        wallDetected = core.CollisionSenses.WallFront;
    }

    public override void Enter()
    {
        base.Enter();

        performCloseRangeAction = false;
        isChargeTimeOver = false;
        core.Movement.SetVelocityX(stateData.chargeSpeed * core.Movement.FacingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.SetVelocityX(stateData.chargeSpeed * core.Movement.FacingDirection);

        if (Time.time >= startTime + stateData.chargeTime)
        {
            isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

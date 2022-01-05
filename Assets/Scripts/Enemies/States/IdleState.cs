using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected Data_IdleState stateData;

    protected bool flipAfterIdle;
    protected float idleTime;
    protected bool isPlayerInMinAggroRange;
    
    protected bool isIdleTimeOver;

    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isPlayerInMinAggroRange = entity.CheckPlayerInMinAggroRange();
    }

    public override void Enter()
    {
        base.Enter();

        core.Movement.SetVelocityX(0);
        isIdleTimeOver = false;
        SetRandomIdleTime();
    }

    public override void Exit()
    {
        base.Exit();

        if (flipAfterIdle)
        {
            core.Movement.Flip();
            flipAfterIdle = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        core.Movement.SetVelocityX(0);

        if (Time.time >= startTime + idleTime)
        {
            isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetFlipAfterIdle(bool flip)
    {
        flipAfterIdle = flip;
    }

    void SetRandomIdleTime()
    {
        idleTime = Random.Range(stateData.minIdleTime, stateData.maxIdleTime);
    }
}

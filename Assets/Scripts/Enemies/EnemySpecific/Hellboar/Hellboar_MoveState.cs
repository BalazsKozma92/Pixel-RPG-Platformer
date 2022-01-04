using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellboar_MoveState : MoveState
{
    Hellboar enemy;

    public Hellboar_MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_MoveState stateData, Hellboar enemy) : base(entity, stateMachine, animBoolName, stateData)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPlayerInMinAggroRange)
        {
            stateMachine.ChangeState(enemy.playerDetectedState);
        }
        else if (wallDetected || !ledgeDetected)
        {
            enemy.idleState.SetFlipAfterIdle(true);
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

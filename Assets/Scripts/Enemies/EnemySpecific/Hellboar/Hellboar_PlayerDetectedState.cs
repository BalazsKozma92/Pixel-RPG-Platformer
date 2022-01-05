using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellboar_PlayerDetectedState : PlayerDetectedState
{
    Hellboar enemy;

    public Hellboar_PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Data_PlayerDetectedState stateData, Hellboar enemy) : base(entity, stateMachine, animBoolName, stateData)
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

        if (performCloseRangeAction)
        {
            stateMachine.ChangeState(enemy.meleeAttackState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(enemy.chargeState);
        }
        else if (!isPlayerInMaxAggroRange)
        {
            stateMachine.ChangeState(enemy.lookForPlayerState);
        }
        else if (!ledgeDetected)
        {
            core.Movement.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellboar : Entity
{
    public Hellboar_IdleState idleState { get; private set; }
    public Hellboar_MoveState moveState { get; private set; }
    public Hellboar_PlayerDetectedState playerDetectedState { get; private set; }
    public Hellboar_ChargeState chargeState { get; private set; }
    public Hellboar_LookForPlayerState lookForPlayerState { get; private set; }
    public Hellboar_MeleeAttackState meleeAttackState { get; private set; }
    public Hellboar_StunState stunState { get; private set; }
    public Hellboar_DeadState deadState { get; private set; }

    [SerializeField] Data_IdleState idleStateData;
    [SerializeField] Data_MoveState moveStateData;
    [SerializeField] Data_PlayerDetectedState playerDetectedStateData;
    [SerializeField] Data_ChargeState chargeStateData;
    [SerializeField] Data_LookForPlayerState lookForPlayerStateData;
    [SerializeField] Data_MeleeAttackState meleeAttackStateData;
    [SerializeField] Transform meleeAttackPosition;
    [SerializeField] Data_StunState stunStateData;
    [SerializeField] Data_DeadState deadStateData;

    public override void Awake()
    {
        base.Awake();

    }

    public override void Start()
    {
        base.Start();
        moveState = new Hellboar_MoveState(this, stateMachine, "move", moveStateData, this);
        idleState = new Hellboar_IdleState(this, stateMachine, "idle", idleStateData, this);
        playerDetectedState = new Hellboar_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        chargeState = new Hellboar_ChargeState(this, stateMachine, "charge", chargeStateData, this);
        lookForPlayerState = new Hellboar_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new Hellboar_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new Hellboar_StunState(this, stateMachine, "stunned", stunStateData, this);
        deadState = new Hellboar_DeadState(this, stateMachine, "dead", deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(float amount, int direction)
    {
        base.Damage(amount, direction);

        if (isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if (isStunned && stateMachine.currentState != stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if (!CheckPlayerInMinAggroRange() && !isStunned)
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public void Getthis()
    {

    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorm : Entity
{
    public FireWorm_IdleState idleState { get; private set; }
    public FireWorm_MoveState moveState { get; private set; }
    public FireWorm_PlayerDetectedState playerDetectedState { get; private set; }
    public FireWorm_LookForPlayerState lookForPlayerState { get; private set; }
    public FireWorm_MeleeAttackState meleeAttackState { get; private set; }
    public FireWorm_StunState stunState { get; private set; }
    public FireWorm_DeadState deadState { get; private set; }
    public FireWorm_DodgeState dodgeState { get; private set; }
    public FireWorm_RangedAttackState rangedAttackState { get; private set; }

    [SerializeField] Data_IdleState idleStateData;
    [SerializeField] Data_MoveState moveStateData;
    [SerializeField] Data_PlayerDetectedState playerDetectedStateData;
    [SerializeField] Data_LookForPlayerState lookForPlayerStateData;
    [SerializeField] Data_MeleeAttackState meleeAttackStateData;
    [SerializeField] Transform meleeAttackPosition;
    [SerializeField] Data_StunState stunStateData;
    [SerializeField] Data_DeadState deadStateData;
    [SerializeField] public Data_DodgeState dodgeStateData;
    [SerializeField] Data_RangedAttackState rangedAttackStateData;
    [SerializeField] Transform rangedAttackPosition;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();


        idleState = new FireWorm_IdleState(this, stateMachine, "idle", idleStateData, this);
        moveState = new FireWorm_MoveState(this, stateMachine, "move", moveStateData, this);
        playerDetectedState = new FireWorm_PlayerDetectedState(this, stateMachine, "playerDetected", playerDetectedStateData, this);
        lookForPlayerState = new FireWorm_LookForPlayerState(this, stateMachine, "lookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new FireWorm_MeleeAttackState(this, stateMachine, "meleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new FireWorm_StunState(this, stateMachine, "stunned", stunStateData, this);
        deadState = new FireWorm_DeadState(this, stateMachine, "dead", deadStateData, this);
        dodgeState = new FireWorm_DodgeState(this, stateMachine, "dodge", dodgeStateData, this);
        rangedAttackState = new FireWorm_RangedAttackState(this, stateMachine, "rangedAttack", rangedAttackPosition, rangedAttackStateData, this);
        stateMachine.Initialize(moveState);
    }

    // public override void Damage(AttackDetails attackDetails)
    // {
    //     base.Damage(attackDetails);

    //     if (isDead)
    //     {
    //         stateMachine.ChangeState(deadState);
    //     }
    //     else if (isStunned && stateMachine.currentState != stunState)
    //     {
    //         stateMachine.ChangeState(stunState);
    //     }
    //     else if (CheckPlayerInMinAggroRange())
    //     {
    //         stateMachine.ChangeState(rangedAttackState);
    //     }
    //     else if (!CheckPlayerInMinAggroRange() && !isStunned)
    //     {
    //         lookForPlayerState.SetTurnImmediately(true);
    //         stateMachine.ChangeState(lookForPlayerState);
    //     }
    // }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData.attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(rangedAttackPosition.position, .2f);
    }
}

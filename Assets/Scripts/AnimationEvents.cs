using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void AttackPlayer() 
    {
        GetComponentInParent<EnemyCombat>().AttackPlayer();
    }

    public void Landed()
    {
        // Player.Instance.LandEffect();
    }

    // public void AttackEnemy()
    // {
    //     GetComponentInParent<PlayerAttack>().CheckForEnemies(1f, 1f, false);
    // }

    // public void KickAttackEnemy()
    // {
    //     GetComponentInParent<PlayerAttack>().CheckForEnemies(1.25f, 1f, false);
    // }

    // public void RayBurst()
    // {
    //     GetComponentInParent<PlayerAttack>().CheckForEnemies(1.45f, 3f, true);
    // }

    // public void AttackWithBow()
    // {
    //     GetComponentInParent<PlayerAttack>().ShootingArrow(1f, 1f, false);
    // }

    public void ClimbedOnLedge()
    {
        // Player.Instance.FinishLedgeClimb();
    }
}

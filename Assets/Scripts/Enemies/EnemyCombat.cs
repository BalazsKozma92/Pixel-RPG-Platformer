using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] int attackDamage = 1;
    [SerializeField] bool isTrap = false;
    [SerializeField] float attackRange = 1.2f;
    [SerializeField] float timeBetweenAttacks = 1f;
    Animator animator;

    PlayerBase player;

    float attackTimer = 0;
    bool attackTimerStarted = false;

    void Awake() 
    {
        player = FindObjectOfType<PlayerBase>();
        if (!isTrap) 
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    void Update() 
    {
        if (isTrap) {return;}

        if (attackTimerStarted) 
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0) 
            {
                attackTimerStarted = false;
            }
        }

        if (Vector2.Distance(transform.position, player.transform.position) < attackRange) 
        {
            if (attackTimer <= 0) 
            {
                attackTimer = timeBetweenAttacks;
                attackTimerStarted = true;
                animator.SetTrigger("attack");
            }
        } 
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player") && isTrap) 
        {
            // PlayerBase.Instance.GetHurt(attackDamage);
            // other.GetComponent<PlayerStats>().Hit(attackDamage, isTrap);
        }
    }

    public void AttackPlayer() 
    {
        int targetSide = transform.position.x < player.transform.position.x ? 1 : -1;
        if (GetComponentInChildren<SpriteRenderer>().transform.localScale.x == targetSide && Vector2.Distance(transform.position, player.transform.position) < attackRange)
        {
            // PlayerBase.Instance.GetHurt(targetSide, attackDamage);
        }
        // player.Hit(attackDamage, isTrap);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    bool isKnockbackActive;
    float knockbackStartTime;
    [SerializeField] float maxKnockbackTime = 0.2f;
    [SerializeField] GameObject deathBloodParticles;
    [SerializeField] GameObject deathChunkParticles;
    [SerializeField] GameObject bloodParticles;
    [SerializeField] float maxHealth;

    float currentHealth;
    Entity thisEntity;

    protected override void Awake()
    {
        base.Awake();

        thisEntity = core.transform.parent.GetComponent<Entity>();
    }

    void Start()
    {
        currentHealth = maxHealth;    
    }

    public void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount, int direction)
    {
        if (thisEntity != null)
        {
            thisEntity.Damage(amount, direction);
        }
        else
        {
            currentHealth -= amount;
            Instantiate(bloodParticles, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            if (currentHealth <= 0)
            {
                GameObject.Instantiate(deathBloodParticles, transform.position, deathBloodParticles.transform.rotation);
                GameObject.Instantiate(deathChunkParticles, transform.position, deathChunkParticles.transform.rotation);
                transform.parent.parent.gameObject.SetActive(false);
            }
        }
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        if (thisEntity != null && thisEntity.GetIsStunned())
        {
            angle = new Vector2(0, angle.y);
        }
        core.Movement.SetVelocity(strength, angle, direction);
        core.Movement.CanSetVelocity = false;

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    void CheckKnockback()
    {
        if (isKnockbackActive && core.Movement.CurrentVelocity.y <= 0.01f && (core.CollisionSenses.Ground || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            core.Movement.CanSetVelocity = true;
        }
    }
}

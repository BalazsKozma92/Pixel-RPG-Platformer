using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    bool isKnockbackActive;
    float knockbackStartTime;
    [SerializeField] float maxKnockbackTime = 0.2f;

    public void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount)
    {
        Debug.Log(core.transform.parent.name + " is damaged.");
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
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

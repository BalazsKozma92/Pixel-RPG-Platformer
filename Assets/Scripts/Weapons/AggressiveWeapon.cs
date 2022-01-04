using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected AggressiveWeaponData aggressiveWeaponData;
    List<IDamageable> detectedDamageables = new List<IDamageable>();

    protected override void Awake()
    {
        base.Awake();

        if (weaponData.GetType() == typeof(AggressiveWeaponData))
        {
            aggressiveWeaponData = (AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for the weapon");
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }

    void CheckMeleeAttack()
    {
        WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];

        foreach (IDamageable item in detectedDamageables)
        {
            item.Damage(details.damageAmount);
        }
    }

    public void AddToDetected(Collider2D other)
    {
        Debug.Log("Adding now");

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("added succesfully");
            detectedDamageables.Add(damageable);
        }
    }

    public void RemoveFromDetected(Collider2D other)
    {
        Debug.Log("Removing now");
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("removed succesfully");
            detectedDamageables.Remove(damageable);
        }
    }
}

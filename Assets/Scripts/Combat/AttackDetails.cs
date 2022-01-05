using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttackDetails
{   
    public string weaponTypeName;

    [Header("Attack")]
    public string attackName;
    public float movementSpeed;
    public float damageAmount;
    public float stunDamageAmount;

    [Header("Knockback")]
    public float knockbackStrength;
    public Vector2 knockbackAngle;
}
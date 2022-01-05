using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttackDetails
{
    [Header("Attack")]
    public string attackName;
    public float movementSpeed;
    public float damageAmount;

    [Header("Knockback")]
    public float knockbackStrength;
    public Vector2 knockbackAngle;
}
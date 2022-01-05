using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee attack state")]
public class Data_MeleeAttackState : ScriptableObject
{
    [Header("Attack")]
    public float attackRadius = .5f;
    public float attackDamage = 1f;

    [Header("Knockback")]
    public float knockbackStrength = 8f;
    public Vector2 knockbackAngle = new Vector2(1, 2);

    public LayerMask playerMask;
}

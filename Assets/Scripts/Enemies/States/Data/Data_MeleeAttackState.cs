using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee attack state")]
public class Data_MeleeAttackState : ScriptableObject
{
    public float attackRadius = .5f;
    public float attackDamage = 1f;

    public LayerMask playerMask;
}

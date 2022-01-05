using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged attack state")]
public class Data_RangedAttackState : ScriptableObject
{
    public GameObject projectile;
    public float projectileDamage = 2f;
    public float projectileSpeed = 14f;
    public float projectileTravelDistance;
    public Vector2 knockbackAngle = new Vector2(1, 1);
    public float knockbackStrength = 5f;
}

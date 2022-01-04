using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class Data_Entity : ScriptableObject
{
    [Header("Check data")]
    public float wallCheckDistance = .2f;
    public float ledgeCheckDistance = .9f;
    public float groundCheckRadius = .3f;

    [Header("Health data")]
    public float maxHealth = 5f;

    [Header("Combat data")]
    public GameObject hitParticles;
    public float minAggroDistance = 6f;
    public float maxAggroDistance = 7.5f;
    public float closeRangeActionDistance = 1f;
    public float damageHopSpeed = 10f;

    [Header("Stun data")]
    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;

    [Header("Layermasks")]
    public LayerMask groundMask;
    public LayerMask playerMask;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/Weapon data/Aggressive Weapon")]
public class AggressiveWeaponData : WeaponData
{
    [SerializeField] WeaponAttackDetails[] attackDetails;

    public WeaponAttackDetails[] AttackDetails { get => attackDetails; set => attackDetails = value; }

    void OnEnable()
    {
        amountOfAttacks = attackDetails.Length;

        movementSpeed = new float[amountOfAttacks];

        for (int i = 0; i < amountOfAttacks; i++)
        {
            movementSpeed[i] = attackDetails[i].movementSpeed;
        }    
    }
}

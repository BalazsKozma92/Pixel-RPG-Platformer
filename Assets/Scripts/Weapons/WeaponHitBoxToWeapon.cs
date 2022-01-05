using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBoxToWeapon : MonoBehaviour
{
    AggressiveWeapon weapon;

    void Awake()
    {
        weapon = GetComponentInParent<AggressiveWeapon>();    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        weapon.AddToDetected(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        weapon.RemoveFromDetected(other);    
    }
}

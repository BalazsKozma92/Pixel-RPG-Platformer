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
        Debug.Log("OnTriggerEnter2D");
        weapon.AddToDetected(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D");
        weapon.RemoveFromDetected(other);    
    }
}

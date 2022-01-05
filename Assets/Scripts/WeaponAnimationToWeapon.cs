using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationToWeapon : MonoBehaviour
{
    Weapon weapon;
    
    void Awake()
    {
        weapon = GetComponentInParent<Weapon>();    
    }

    void AnimationFinishTrigger()
    {
        weapon.AnimationFinishTrigger();
    }

    void AnimationStartMovementTrigger()
    {
        weapon.AnimationStartMovementTrigger();
    }

    void AnimationStopMovementTrigger()
    {
        weapon.AnimationStopMovementTrigger();
    }

    void AnimationActionTrigger()
    {
        weapon.AnimationActionTrigger();
    }

    void PlaySwordSwingAudio()
    {
        AudioPlayer.Instance.PlaySwingSound();
    }
}

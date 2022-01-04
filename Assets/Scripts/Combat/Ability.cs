using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Ability : MonoBehaviour
{
    Image myImage;
    AbilityManager abilityManager;
    Gems gems;
    Abilities abilities;

    KeyControl abilityKey;
    bool abilityActive = false;
    float cooldown;
    float remainingCooldown;

    private void Awake()
    {
        myImage = GetComponent<Image>();
        abilityManager = GetComponentInParent<AbilityManager>();
        gems = FindObjectOfType<Gems>();
        abilities = FindObjectOfType<Abilities>();
    }

    private void Start()
    {
        gems.onCooldownChange += ChangeCooldown;
    }

    private void Update()
    {
        if (myImage.enabled && !abilityActive)
        {
            abilityActive = true;

            if (abilityManager.transform.GetChild(0) == transform.parent) { abilityKey = Keyboard.current.digit1Key; } 
            else if (abilityManager.transform.GetChild(1) == transform.parent) { abilityKey = Keyboard.current.digit2Key; }
            else if (abilityManager.transform.GetChild(2) == transform.parent) { abilityKey = Keyboard.current.digit3Key; }
            else if (abilityManager.transform.GetChild(3) == transform.parent) { abilityKey = Keyboard.current.digit4Key; }

            cooldown = gems.vigourCoroutineTimers[gems.abilityNameToSocketName[myImage.sprite.name]];
        }
        else if (!myImage.enabled && abilityActive)
        {
            abilityActive = false;
        }

        if (abilityActive && abilityKey.wasPressedThisFrame)
        {
            ActivateAbility(myImage.sprite.name);
        }
    }

    void ActivateAbility(string abilityName)
    {
        abilityManager.StartAbility(abilityName, cooldown, transform);
    }

    void ChangeCooldown(string gemType)
    {
        if (gemType == "vigour")
        {   
            if (abilities.IsKeyInDict(myImage.sprite.name))
            {
                cooldown = gems.vigourCoroutineTimers[gems.abilityNameToSocketName[myImage.sprite.name]];
            }
        }
        if (gemType == "death")
        {   
            if (abilities.IsKeyInDict(myImage.sprite.name))
            {
                cooldown = gems.deathCoroutineTimers[gems.abilityNameToSocketName[myImage.sprite.name]];
            }
        }
        if (gemType == "balance")
        {   
            if (abilities.IsKeyInDict(myImage.sprite.name))
            {
                cooldown = gems.balanceCoroutineTimers[gems.abilityNameToSocketName[myImage.sprite.name]];
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    Abilities abilities;

    float meditateCooldown;
    float currentMeditateCooldown;
    bool meditateCanBeUsed = true;
    Image meditateCDImage;

    float healthFocusCooldown;
    float currentHealthFocusCooldown;
    bool healthFocusCanBeUsed = true;
    Image healthFocusCDImage;

    float rayBurstCooldown;
    float currentRayBurstCooldown;
    bool rayBurstCanBeUsed = true;
    Image rayBurstCDImage;

    float kickCooldown;
    float currentKickCooldown;
    bool kickCanBeUsed = true;
    Image kickCDImage;

    private void Awake()
    {
        abilities = FindObjectOfType<Abilities>();
    }

    private void Update() {
        if (!meditateCanBeUsed)
        {
            currentMeditateCooldown -= Time.deltaTime;
            meditateCDImage.fillAmount = currentMeditateCooldown / meditateCooldown;
            if (currentMeditateCooldown <= 0)
            {
                meditateCanBeUsed = true;
            }
        }
        if (!healthFocusCanBeUsed)
        {
            currentHealthFocusCooldown -= Time.deltaTime;
            healthFocusCDImage.fillAmount = currentHealthFocusCooldown / healthFocusCooldown;
            if (currentHealthFocusCooldown <= 0)
            {
                healthFocusCanBeUsed = true;
            }
        }
        if (!rayBurstCanBeUsed)
        {
            currentRayBurstCooldown -= Time.deltaTime;
            rayBurstCDImage.fillAmount = currentRayBurstCooldown / rayBurstCooldown;
            if (currentRayBurstCooldown <= 0)
            {
                rayBurstCanBeUsed = true;
            }
        }
        if (!kickCanBeUsed)
        {
            currentKickCooldown -= Time.deltaTime;
            kickCDImage.fillAmount = currentKickCooldown / kickCooldown;
            if (currentKickCooldown <= 0)
            {
                kickCanBeUsed = true;
            }
        }
    }

    public void StartAbility(string abilityName, float cooldown, Transform abilityTransform)
    {
        switch (abilityName)
        {
            case "Meditate":
                meditateCDImage = abilityTransform.GetChild(0).GetComponent<Image>();
                meditateCooldown = cooldown;
                AbilityMeditate();
                break;
            case "HealthFocus":
                healthFocusCDImage = abilityTransform.GetChild(0).GetComponent<Image>();
                healthFocusCooldown = cooldown;
                AbilityHealthFocus();
                break;
            case "RayBurst":
                rayBurstCDImage = abilityTransform.GetChild(0).GetComponent<Image>();
                rayBurstCooldown = cooldown;
                AbilityRayBurst();
                break;
            case "Kick":
                kickCDImage = abilityTransform.GetChild(0).GetComponent<Image>();
                kickCooldown = cooldown;
                AbilityKick();
                break;
        }
    }

    void AbilityMeditate()
    {
        if (meditateCanBeUsed)
        {
            meditateCanBeUsed = false;
            currentMeditateCooldown = meditateCooldown;
        }
    }

    void AbilityHealthFocus()
    {
        if (healthFocusCanBeUsed)
        {
            healthFocusCanBeUsed = false;
            currentHealthFocusCooldown = healthFocusCooldown;
        }
    }

    void AbilityRayBurst()
    {
        if (rayBurstCanBeUsed)
        {
            // PlayerBase.Instance.GetComponentInChildren<Animator>().SetTrigger("cast");
            rayBurstCanBeUsed = false;
            currentRayBurstCooldown = rayBurstCooldown;
        }
    }

    void AbilityKick()
    {
        if (kickCanBeUsed)
        {
            // PlayerBase.Instance.GetComponentInChildren<Animator>().SetTrigger("kick");
            kickCanBeUsed = false;
            currentKickCooldown = kickCooldown;
        }
    }
}

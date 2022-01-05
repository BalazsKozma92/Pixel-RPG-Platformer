using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GemBuffManager : MonoBehaviour
{
//////////////////////////////////////////////////////BASE FUNCTIONS/////////////////////////////////////////////

    [SerializeField] GameObject abilityRowHider;
    [SerializeField] Sprite placeholderSprite;

    Animator animator;
    CameraShake cameraShake;
    PlayerInputHandler playerInputHandler;
    Combat playerCombat;
    Sprite imageToPutBack;

    // AbilityMana
    int allHealthFromVigour = 0;
    int allDamageFromDeath = 0;
    float allAttackDamageFromVigour = 0;
    float allAttackDamageFromDeath = 0;
    float phoenixGemCD;

    bool isInCombat;
    bool respawnOnDeathHalf = false;
    bool respawnOnDeathFull = false;

    public event Action<float> HealthOrbDropIncreased;
    // public event Action ResetAllVigourCD;

    static GemBuffManager instance;
    public static GemBuffManager Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<GemBuffManager>();
            return instance;
        }
    }

    private void Awake()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        animator = GetComponent<Animator>();
        playerInputHandler = FindObjectOfType<PlayerInputHandler>();
        playerCombat = playerInputHandler.GetComponentInChildren<Combat>();
    }

    private void Start()
    {
        // playerAttack.OnProccingCDReset += ResetTypeCD;
    }

//////////////////////////////////////////////////////GEM FUNCTIONS/////////////////////////////////////////////

    public void SetJumpStrength(float strength)
    {
        
        // PlayerBase.Instance.SetJumpPower(strength);
    }

    public void SetAccumulativeHealthByDeath(int health)
    {
        allDamageFromDeath += health;
    }

    public void SetAccumulativeHealthByVigour(int health)
    {
        allHealthFromVigour += health;
    }

    public void SetAccumulativeAttackDamageByVigour(float attackDamage)
    {
        allAttackDamageFromVigour += attackDamage;
        // playerAttack.SetAttackDamage(attackDamage);
    }

    public void SetAccumulativeAttackDamageByDeath(int attackDamage)
    {
        allAttackDamageFromDeath += attackDamage;
        // playerAttack.SetAttackDamage(attackDamage);
    }

    public int GetBonusVigourHealth()
    {
        return allHealthFromVigour;
    }

    public int GetDamageFromDeath()
    {
        return allDamageFromDeath;
    }

    public float GetBonusVigourAttackDamage()
    {
        return allAttackDamageFromVigour;
    }

    public float GetBonusDeathAttackDamage()
    {
        return allAttackDamageFromDeath;
    }

    public void SetMaxHealth(float value) {
        playerCombat.SetMaxHealth(value);
    }

    public IEnumerator RegenerateOverTime(float amount, float overTime) {
        while (true) {
            playerCombat.SetHealth(amount);
            yield return new WaitForSeconds(overTime);
        }
    }

    public IEnumerator CountDownPhoenixCD(float cooldown, string parentObject) {
        phoenixGemCD = cooldown;
        while (phoenixGemCD > 0) {
            respawnOnDeathHalf = false;
            respawnOnDeathFull = false;
            phoenixGemCD -= 1f;
            yield return new WaitForSeconds(1);
        }
        if (parentObject == "half")
        {
            respawnOnDeathHalf = true;
        }
        else if (parentObject == "full")
        {
            respawnOnDeathFull = true;
        }
    }

    public void IncreaseHealthDropChance(float increaseWith)
    {
        HealthOrbDropIncreased(increaseWith);
    }

    public void AddChanceToCDReset(float percentage, string gemType)
    {
        // playerAttack.SetChanceToResetCD(percentage, gemType);
    }

    public string GetRespawnOnDeath()
    {
        if (respawnOnDeathFull)
        {
            return "full";
        }
        else if (respawnOnDeathHalf)
        {
            return "half";
        }
        return "";
    }

    void ResetTypeCD(List<string> gemTypesToReset) {
        foreach (string gemTypeName in gemTypesToReset)
        {
            GetComponent<Gems>().ResetCoroutines(gemTypeName);
        }
    }

    public void SetAbilityActive(string abilityName, bool active)
    {
        Ability[] chosenAbilityIcons = GameObject.FindObjectsOfType<Ability>();
        GameObject[] availableAbilities = GameObject.FindGameObjectsWithTag("Ability");

        for (int i = 0; i < availableAbilities.Length; i++)
        {
            if (availableAbilities[i].GetComponent<Image>().enabled == false)
            {
                availableAbilities[i].GetComponent<Image>().sprite = placeholderSprite;
                availableAbilities[i].name = "Ability";
            }
        }
        for (int i = 0; i < chosenAbilityIcons.Length; i++)
        {
            if (chosenAbilityIcons[i].GetComponent<Image>().enabled == false)
            {
                chosenAbilityIcons[i].GetComponent<Image>().sprite = placeholderSprite;
                chosenAbilityIcons[i].name = "Ability";
            }
        }

        if (active)
        {
            for (int i = 0; i < availableAbilities.Length; i++)
            {
                if (availableAbilities[i].name == abilityName)
                {
                    abilityRowHider.transform.GetChild(i).GetComponent<Image>().enabled = false;
                }
            }
        }
        else if (!active)
        {
            for (int i = 0; i < chosenAbilityIcons.Length; i++)
            {
                if (chosenAbilityIcons[i].name == abilityName)
                {
                    imageToPutBack = chosenAbilityIcons[i].GetComponent<Image>().sprite;
                    chosenAbilityIcons[i].GetComponent<Image>().enabled = false;
                    chosenAbilityIcons[i].name = "Ability";
                    break;
                }
            }
            for (int i = 0; i < availableAbilities.Length; i++)
            {
                if (availableAbilities[i].name == abilityName && availableAbilities[i].GetComponent<Image>().enabled)
                {
                    abilityRowHider.transform.GetChild(i).GetComponent<Image>().enabled = true;
                    break;
                }
                else if (i == availableAbilities.Length - 1)
                {
                    for (int j = 0; j < availableAbilities.Length; j++)
                    {
                        if (availableAbilities[j].name == "Ability")
                        {
                            availableAbilities[j].GetComponent<Image>().enabled = true;
                            availableAbilities[j].name = abilityName;
                            availableAbilities[j].GetComponent<Image>().sprite = imageToPutBack;
                            abilityRowHider.transform.GetChild(j).GetComponent<Image>().enabled = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}

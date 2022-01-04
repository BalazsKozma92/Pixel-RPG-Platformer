using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
// using System;

public class InventorySlotUI : MonoBehaviour, IDragContainer<Sprite>
{
    // CONFIG DATA
    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] AbilityItemIcon abilityIcon = null;
    [SerializeField] Gems gems;
    [SerializeField] Abilities abilities;

    ToolTipTrigger tooltipTrigger;
    Sprite itemIcon;
    Sprite abilityItemIcon;
    Sprite currentItem;
    Sprite currentAbilityItem;
    bool initializing = true;
    bool abilityInitializing = true;

    void ReDrawToolTip()
    {
        if (GetComponentInChildren<InventoryItemIcon>())
        {
            itemIcon = GetComponentInChildren<InventoryItemIcon>().GetComponent<Image>().sprite;
            if (itemIcon && gems.IsKeyInDict(itemIcon.name))
            {
                string[] gemDetails = gems.GetTooltip(itemIcon.name);
                tooltipTrigger.SetText(gemDetails[0], gemDetails[1], gemDetails[2], gemDetails[3], gemDetails[4], gemDetails[5]);
            }
        }

        if (GetComponentInChildren<AbilityItemIcon>())
        {
            abilityItemIcon = GetComponentInChildren<AbilityItemIcon>().GetComponent<Image>().sprite;
            if (abilityItemIcon && abilities.IsKeyInDict(abilityItemIcon.name))
            {
                string[] abilityDetails = abilities.GetTooltip(abilityItemIcon.name);
                tooltipTrigger.SetText(abilityDetails[0], abilityDetails[1], "","","","", true);
            }
        }
    }

    private void Start() {
        tooltipTrigger = GetComponent<ToolTipTrigger>();

        if (GetComponentInChildren<InventoryItemIcon>())
        {
            itemIcon = GetComponentInChildren<InventoryItemIcon>().GetComponent<Image>().sprite;
            if (itemIcon) {
                AddItems(itemIcon, 1);
            }
            initializing = false;
        }
        if (GetComponentInChildren<AbilityItemIcon>())
        {
            abilityItemIcon = GetComponentInChildren<AbilityItemIcon>().GetComponent<Image>().sprite;
            if (abilityItemIcon) {
                AddAbilityItems(abilityItemIcon, 1);
            }
            abilityInitializing = false;
        }

        gems.reDrawToolTips += ReDrawToolTip;

        if (GetComponentInChildren<AbilityItemIcon>())
        {
            GetComponentInChildren<AbilityItemIcon>().name = GetComponentInChildren<AbilityItemIcon>().GetComponent<Image>().sprite.name;
        }
    }

    public int MaxAcceptable(Sprite item)
    {
        return int.MaxValue;
    }

    public void AddItems(Sprite item, int number)
    {
        if (!initializing)
        {
            AudioPlayer.Instance.PlayGemSocketed();
        }
        currentItem = item;
        gems.InitDicts(false);
        if (gems.IsKeyInDict(item.name))
        {
            string[] gemDetails = gems.GetTooltip(item.name);
            gems.ReCalculateBuffs(item.name, transform, true, "");
            tooltipTrigger.SetText(gemDetails[0], gemDetails[1], gemDetails[2], gemDetails[3], gemDetails[4], gemDetails[5]);
            icon.SetItem(item);
        }
        ReDrawToolTip();
    }

    public void AddAbilityItems(Sprite item, int number)
    {
        StartCoroutine(DelayedAbilityInit(item, number));
        // currentAbilityItem = item;
        // abilities.InitDicts(false);
        // if (abilities.IsKeyInDict(item.name))
        // {
        //     string[] abilityDetails = abilities.GetTooltip(item.name);
        //     // abilities.ReCalculateBuffs(item.name, transform, true, "");
        //     tooltipTrigger.SetText(abilityDetails[0], abilityDetails[1], "","","","", true);
        //     abilityIcon.SetItem(item);
        // }
        // ReDrawAbilityToolTip();
    }

    IEnumerator<WaitForSeconds> DelayedAbilityInit(Sprite item, int number)
    {
        yield return new WaitForSeconds(.1f);
        currentAbilityItem = item;
        abilities.InitDicts(false);
        if (abilities.IsKeyInDict(item.name))
        {
            string[] abilityDetails = abilities.GetTooltip(item.name);
            // abilities.ReCalculateBuffs(item.name, transform, true, "");
            tooltipTrigger.SetText(abilityDetails[0], abilityDetails[1], "","","","", true);
            abilityIcon.SetItem(item);
        }
        ReDrawToolTip();
    }

    public Sprite GetItem()
    {   
        return icon.GetItem(); 
    }

    public Sprite GetAbilityItem()
    {
        return abilityIcon.GetItem(); 
    }

    public int GetNumber()
    {
        return 1;
    }

    public void RemoveItems(int number)
    {
        tooltipTrigger.SetText("","","","","","");
        gems.ReCalculateBuffs(currentItem.name, transform, false, "");
        icon.SetItem(null);
    }

    public void RemoveAbilityItems(int number)
    {
        tooltipTrigger.SetText("","","","","","", true);
        // gems.ReCalculateBuffs(currentItem.name, transform, false, "");
        abilityIcon.SetItem(null);
    }

    public void ChangeSourceAbilitySocketName()
    {
        StartCoroutine(DelayedSourceSocketNameChange());
        // GetComponentInChildren<AbilityItemIcon>().name = GetComponentInChildren<AbilityItemIcon>().GetComponent<Image>().sprite.name;
    }

    IEnumerator<WaitForSeconds> DelayedSourceSocketNameChange()
    {
        yield return new WaitForSeconds(.11f);
        GetComponentInChildren<AbilityItemIcon>().name = GetComponentInChildren<AbilityItemIcon>().GetComponent<Image>().sprite.name;
    }

    public void ChangeDestinationAbilitySocketName()
    {
        StartCoroutine(DelayedDestSocketNameChange());
        // GetComponentInChildren<AbilityItemIcon>().name = GetComponentInChildren<AbilityItemIcon>().GetComponent<Image>().sprite.name;
    }

    IEnumerator<WaitForSeconds> DelayedDestSocketNameChange()
    {
        yield return new WaitForSeconds(.11f);
        GetComponentInChildren<AbilityItemIcon>().name = GetComponentInChildren<AbilityItemIcon>().GetComponent<Image>().sprite.name;
    }
}
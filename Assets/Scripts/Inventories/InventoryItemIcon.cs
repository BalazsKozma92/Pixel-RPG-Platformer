using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class InventoryItemIcon : MonoBehaviour
{
    // PUBLIC

    public void SetItem(Sprite item)
    {
        var iconImage = GetComponent<Image>();
        if (item == null)
        {
            iconImage.enabled = false;
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = item;
        }
    }

    public Sprite GetItem()
    {
        var iconImage = GetComponent<Image>();
        if (!iconImage.enabled)
        {
            return null;
        }
        return iconImage.sprite;
    }
}
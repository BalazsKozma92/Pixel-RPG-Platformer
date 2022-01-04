using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowHandler : MonoBehaviour
{
    [SerializeField] Canvas perksCanvas;
    [SerializeField] Canvas statsCanvas;
    [SerializeField] Canvas questsCanvas;
    [SerializeField] Canvas questItemsCanvas;

    [SerializeField] Toggle perksButton;
    [SerializeField] Toggle statsButton;
    [SerializeField] Toggle questsButton;
    [SerializeField] Toggle questItemsButton;

    public void ShowPerksCanvas()
    {
        statsCanvas.enabled = false;
        questsCanvas.enabled = false;
        perksCanvas.enabled = true;
        questItemsCanvas.enabled = false;
        statsButton.SetIsOnWithoutNotify(false);
        questsButton.SetIsOnWithoutNotify(false);
        perksButton.SetIsOnWithoutNotify(true);
        questItemsButton.SetIsOnWithoutNotify(false);
        AudioPlayer.Instance.PlayButtonClick();
    }

    public void ShowStatsCanvas()
    {
        statsCanvas.enabled = true;
        perksCanvas.enabled = false;
        questsCanvas.enabled = false;
        questItemsCanvas.enabled = false;
        perksButton.SetIsOnWithoutNotify(false);
        questsButton.SetIsOnWithoutNotify(false);
        statsButton.SetIsOnWithoutNotify(true);
        questItemsButton.SetIsOnWithoutNotify(false);
        AudioPlayer.Instance.PlayButtonClick();
    }

    public void ShowQuestsCanvas()
    {
        statsCanvas.enabled = false;
        perksCanvas.enabled = false;
        questsCanvas.enabled = true;
        questItemsCanvas.enabled = false;
        questsButton.SetIsOnWithoutNotify(true);
        perksButton.SetIsOnWithoutNotify(false);
        statsButton.SetIsOnWithoutNotify(false);
        questItemsButton.SetIsOnWithoutNotify(false);
        AudioPlayer.Instance.PlayButtonClick();
        questsCanvas.GetComponent<QuestsUI>().SetDescriptionTextToEmpty();
    }

    public void ShowQuestItemCanvas()
    {
        statsCanvas.enabled = false;
        perksCanvas.enabled = false;
        questsCanvas.enabled = false;
        questItemsCanvas.enabled = true;
        perksButton.SetIsOnWithoutNotify(false);
        questsButton.SetIsOnWithoutNotify(false);
        statsButton.SetIsOnWithoutNotify(false);
        questItemsButton.SetIsOnWithoutNotify(true);
        AudioPlayer.Instance.PlayButtonClick();
    }
}

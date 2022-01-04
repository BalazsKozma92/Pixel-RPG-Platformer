using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnnouncementText : MonoBehaviour
{
    [SerializeField] string text;
    [SerializeField] bool addToQuestText;
    [SerializeField] string questName;
    [TextArea(1, 10)]
    [SerializeField] string questTextToAdd;
    [SerializeField] bool isCollectible;

    TextMeshProUGUI announcementText;

    private void Start() {
        announcementText = GameObject.Find("AnnouncementText").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && isCollectible)
        {
            PlayText();
        }    
    }

    public void PlayText()
    {
        if (isCollectible)
        {
            AudioPlayer.Instance.PlayCollection();
            announcementText.text = text;
        }
        if (addToQuestText)
        {
            QuestsUI.Instance.AddToQuestText(questName, questTextToAdd);
        }

        announcementText.GetComponent<Animator>().SetTrigger("Appear");
    }

    public void SetText(string setToThis)
    {
        announcementText.text = setToThis;
        PlayText();
    }
}

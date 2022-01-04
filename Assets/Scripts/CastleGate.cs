using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CastleGate : MonoBehaviour
{
    bool playerIsHere = false;

    private void Update() {
        if (playerIsHere && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (QuestItems.Instance.IsQuestItemInInventory(QuestItem.Village1CastleKey))
            {
                QuestItems.Instance.removeQuestItem(QuestItem.Village1CastleKey);
                GetComponent<Animator>().SetTrigger("open");
                AudioPlayer.Instance.PlayMetalDoorOpen();
            }
            else
            {
                GetComponent<AnnouncementText>().SetText("You don't have the required item.");
                GetComponent<AnnouncementText>().PlayText();
                AudioPlayer.Instance.PlayCantOpen();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerIsHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }
}

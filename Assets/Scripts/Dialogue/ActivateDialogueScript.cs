using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDialogueScript : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            GetComponent<DialogueSystem>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            GetComponent<DialogueSystem>().enabled = false;
        }
    }
}

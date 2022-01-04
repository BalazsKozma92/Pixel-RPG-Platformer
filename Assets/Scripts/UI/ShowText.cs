using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowText : MonoBehaviour
{
    TextMeshProUGUI text;
    Animator animator;

    private void Start() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponentInChildren<Animator>();
        text.alpha = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            animator.SetBool("FadeIn", true);
            animator.SetBool("FadeOut", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        animator.SetBool("FadeOut", true);
        animator.SetBool("FadeIn", false);
    }
}

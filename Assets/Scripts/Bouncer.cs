using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    [SerializeField] float bounceStrength = 3f;

    Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // other.GetComponent<PlayerBase>().Jump(bounceStrength);
            animator.SetTrigger("bounce");
        }
    }
}

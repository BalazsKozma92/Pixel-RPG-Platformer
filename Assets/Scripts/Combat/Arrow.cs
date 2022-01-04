using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    ParticleSystem hitParticles;
    PlayerCombat playerAttack;
    Transform originalParent;

    [System.NonSerialized] public float attackDamage;

    Rigidbody2D rb;
    Vector2 originalParticlePosition;
    bool alreadyUsed = false;
    bool alreadyDisabled = false;

    private void Awake() {
        originalParent = transform.parent;
        rb = GetComponent<Rigidbody2D>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        playerAttack = FindObjectOfType<PlayerCombat>();
    }

    public void AddingVelocityToFly(Vector2 force)
    {
        rb.velocity = force;
        Invoke("DisableSelf", 8f);
        originalParticlePosition = hitParticles.transform.localPosition;
    }

    public void SetAlreadyDisabled(bool disabled)
    {
        alreadyDisabled = disabled;
    }

    public void DisableSelf()
    {
        if (!alreadyDisabled)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            gameObject.SetActive(false);
            // transform.SetParent(originalParent);
            alreadyUsed = false;
            // playerAttack.IncreaseArrowCount(1);
            rb.isKinematic = false;
            GetComponent<Collider2D>().enabled = true;
            alreadyDisabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Platform"))
        {
            alreadyUsed = true;
            GetComponent<Collider2D>().enabled = false;
        }
        if (other.CompareTag("Enemy") || other.CompareTag("Platform"))
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            // transform.SetParent(other.transform);
        }
        if (other.CompareTag("Enemy") && !alreadyUsed)
        {

            AudioPlayer.Instance.PlayBowHitSound();
            hitParticles.transform.SetParent(null);
            hitParticles.Play();
            Invoke("SetParticleParentBack", hitParticles.main.duration);
            // other.GetComponent<EnemyBase>().GetHurt(1, attackDamage);
            GetComponent<Collider2D>().enabled = false;
            DisableSelf();
        }
    }

    void SetParticleParentBack()
    {
        hitParticles.transform.SetParent(transform);
        hitParticles.transform.localPosition = originalParticlePosition;
    }
}


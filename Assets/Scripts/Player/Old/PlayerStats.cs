using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] float maxHealth;
    [SerializeField] GameObject deathChunkParticles;
    [SerializeField] GameObject deathBloodParticles;

    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;    
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;

        AudioPlayer.Instance.PlayHurtSound();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(deathChunkParticles, transform.position, deathChunkParticles.transform.rotation);
        Instantiate(deathBloodParticles, transform.position, deathBloodParticles.transform.rotation);
        GameManager.Instance.Respawn();
        Destroy(gameObject);
    }
}

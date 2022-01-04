using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    AudioSource audioSource;

    [Header("BG Music")]
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] AudioClip villageAmbiance;
    [SerializeField] AudioClip castleAmbiance;
    [SerializeField] AudioClip castlePrisonAmbiance;

    [Header("Player Sound Effects")]
    [SerializeField] AudioClip[] footstepSounds;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] AudioClip[] hurtSounds;
    [SerializeField] AudioClip[] jumpSounds;
    [SerializeField] AudioClip[] landSounds;
    [SerializeField] AudioClip[] swingSounds;
    [SerializeField] AudioClip[] swingHitSounds;
    [SerializeField] AudioClip[] bloodSounds;
    [SerializeField] AudioClip bowSound;
    [SerializeField] AudioClip bowHitSound;
    [SerializeField] AudioClip dashSound;

    [Header("UI Sound Effects")]
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip UIOpen;
    [SerializeField] AudioClip UIClose;
    [SerializeField] AudioClip receiveGold;
    [SerializeField] AudioClip questLogChange;
    [SerializeField] AudioClip dialogueAdvance;
    [SerializeField] AudioClip questAccepted;
    [SerializeField] AudioClip questDone;

    [Header("Gem sounds")]
    [SerializeField] AudioClip gemSocketed;
    [SerializeField] AudioClip gemAcquired;

    [Header("Collection sounds")]
    [SerializeField] AudioClip collection;

    [Header("Gameplay sounds")]
    [SerializeField] AudioClip metalDoorOpen;
    [SerializeField] AudioClip cantOpen;

    [Header("Hellhound sounds")]
    [SerializeField] AudioClip[] hellhoundAttackSounds;
    [SerializeField] AudioClip[] hellhoundAttackHitSounds;
    [SerializeField] AudioClip hellhoundDeathSound;

    private static AudioPlayer instance;

    int gold;
    int vigourFragments;
    int deathFragments;
    int balanceFragments;
    
    public static AudioPlayer Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<AudioPlayer>();
            return instance;
        }
    }

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        audioSource.PlayOneShot(backgroundMusic, .45f);
        audioSource.PlayOneShot(villageAmbiance, .55f);
    }

    public IEnumerator PlayFootsteps()
    {
        while (true)
        {
            int stepSoundIndex = Random.Range(0, footstepSounds.Length);
            audioSource.PlayOneShot(footstepSounds[stepSoundIndex], .5f);
            yield return new WaitForSeconds(.25f);
        }
    }
        
    public void PlayJumpSound()
    {
        int jumpSoundIndex = Random.Range(0, jumpSounds.Length);
        audioSource.PlayOneShot(jumpSounds[jumpSoundIndex]);
    }

    public void PlayBloodSound()
    {
        int bloodSoundIndex = Random.Range(0, bloodSounds.Length);
        audioSource.PlayOneShot(bloodSounds[bloodSoundIndex], .65f);
    }

    public void PlaySwingSound()
    {
        int swingSoundIndex = Random.Range(0, swingSounds.Length);
        audioSource.PlayOneShot(swingSounds[swingSoundIndex], 1f);
    }

    public void PlayHurtSound()
    {
        int hurtSoundIndex = Random.Range(0, hurtSounds.Length);
        audioSource.PlayOneShot(hurtSounds[hurtSoundIndex], .7f);
    }

    public void PlaySwingHitSound()
    {
        int swingHitSoundIndex = Random.Range(0, swingHitSounds.Length);
        audioSource.PlayOneShot(swingHitSounds[swingHitSoundIndex], .7f);
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonClick, 1f);
    }

    public void PlayUIOpen()
    {
        audioSource.PlayOneShot(UIOpen, 1f);
    }

    public void PlayUIClose()
    {
        audioSource.PlayOneShot(UIClose, 1f);
    }

    public void PlayReceiveGold()
    {
        audioSource.PlayOneShot(receiveGold, 1f);
    }

    public void PlayGemSocketed()
    {
        audioSource.PlayOneShot(gemSocketed, 1f);
    }

    public void PlayQuestLogChange()
    {
        audioSource.PlayOneShot(questLogChange, 1f);
    }

    public void PlayNextDialogue()
    {
        audioSource.PlayOneShot(dialogueAdvance, .6f);
    }

    public void PlayQuestAccepted()
    {
        audioSource.PlayOneShot(questAccepted, 1f);
    }

    public void PlayQuestDone()
    {
        audioSource.PlayOneShot(questDone, 1f);
    }

    public void PlayBowSound()
    {
        audioSource.PlayOneShot(bowSound, 1f);
    }

    public void PlayBowHitSound()
    {
        audioSource.PlayOneShot(bowHitSound, 1f);
    }

    public void PlayGemAcquired()
    {
        audioSource.PlayOneShot(gemAcquired, 1f);
    }

    public void PlayCollection()
    {
        audioSource.PlayOneShot(collection, 1f);
    }

    public void PlayMetalDoorOpen()
    {
        audioSource.PlayOneShot(metalDoorOpen, 1f);
    }

    public void PlayCantOpen()
    {
        audioSource.PlayOneShot(cantOpen, 1f);
    }

    public void PlayDash()
    {
        audioSource.PlayOneShot(dashSound, 1f);
    }

    public void PlayHellHoundAttackHitSound()
    {
        int hellhoundAttackHitIndex = Random.Range(0, hellhoundAttackHitSounds.Length);
        audioSource.PlayOneShot(hellhoundAttackHitSounds[hellhoundAttackHitIndex], .65f);
    }

    public void PlayHellHoundAttackSound()
    {
        int hellhoundAttackIndex = Random.Range(0, hellhoundAttackSounds.Length);
        audioSource.PlayOneShot(hellhoundAttackSounds[hellhoundAttackIndex], .65f);
    }

    public void PlayHellHoundDeathSound()
    {
        audioSource.PlayOneShot(hellhoundDeathSound, 1f);
    }
}

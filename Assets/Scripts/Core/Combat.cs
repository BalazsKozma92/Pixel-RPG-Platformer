using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    bool isKnockbackActive;
    float knockbackStartTime;
    [SerializeField] float maxKnockbackTime = 0.2f;
    [SerializeField] GameObject deathBloodParticles;
    [SerializeField] GameObject deathChunkParticles;
    [SerializeField] GameObject bloodParticles;

    [SerializeField] GameObject lifeRow;
    [SerializeField] GameObject lifeBackGroundRow;
    [SerializeField] GameObject lifeBackGroundSprite;
    [SerializeField] GameObject lifeOrbUI;
    [SerializeField] GameObject lifeOrbHalfUI;
    [SerializeField] CanvasGroup screenCanvas;
    [SerializeField] Vector2 respawnCoordinates;
    public float maxHealth { get; set; }
    public float currentHealth { get; private set; }

    bool dead = false;
    bool invulnerable = false;
    float currentInvulnerabilityCounter = 2f;
    float baseInvulnerabilityCounter = 2f;

    Entity thisEntity;
    Tween fadeTween;
    PlayerInputHandler playerInputHandler;
    CameraShake cameraShake;

    public event Action<string> onDeath;

    protected override void Awake()
    {
        base.Awake();

        thisEntity = core.transform.parent.GetComponent<Entity>();
        playerInputHandler = GetComponentInParent<PlayerInputHandler>();
        cameraShake = FindObjectOfType<CameraShake>();
    }

    void Start()
    {
        maxHealth = 15;
        currentHealth = maxHealth;

        if (thisEntity == null)
        {
            RedrawHealthGlobes(); 
        }
    }

    void Update()
    {
        if (invulnerable && currentInvulnerabilityCounter > 0 && thisEntity == null)
        {
            currentInvulnerabilityCounter -= Time.deltaTime;
            if (currentInvulnerabilityCounter <= 0) { invulnerable = false; }
        }    
    }

    public void LogicUpdate()
    {
        CheckKnockback();
    }

    void RedrawHealthGlobes()
    {
        for (int i = 0; i < lifeRow.transform.childCount; i++)
        {
            lifeRow.transform.GetChild(i).gameObject.SetActive(false);
            lifeRow.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            lifeBackGroundRow.transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < ((int)currentHealth / 2); i++)
        {
            lifeRow.transform.GetChild(i).gameObject.SetActive(true);
        }
        if ((int)currentHealth % 2 == 1)
        {
            lifeRow.transform.GetChild((int)currentHealth/2).gameObject.SetActive(true);
            lifeRow.transform.GetChild((int)currentHealth/2).GetChild(0).gameObject.SetActive(false);
        }  
        for (int i = 0; i < (int)maxHealth / 2; i++)
        {
            lifeBackGroundRow.transform.GetChild(i).gameObject.SetActive(true);
        }
        if ((int)maxHealth % 2 == 1)
        {
            lifeBackGroundRow.transform.GetChild((int)maxHealth/2).gameObject.SetActive(true);
        }
    }

    public void Damage(float amount, int direction)
    {
        if (thisEntity != null)
        {
            thisEntity.Damage(amount, direction);
        }
        else
        {
            SetHealth(-amount);
        }
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        if (thisEntity != null && thisEntity.GetIsStunned())
        {
            angle = new Vector2(0, angle.y);
        }
        core.Movement.SetVelocity(strength, angle, direction);
        core.Movement.CanSetVelocity = false;

        isKnockbackActive = true;
        knockbackStartTime = Time.time;
    }

    void CheckKnockback()
    {
        if (isKnockbackActive && core.Movement.CurrentVelocity.y <= 0.01f && (core.CollisionSenses.Ground || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isKnockbackActive = false;
            core.Movement.CanSetVelocity = true;
        }
    }

    public void SetHealth(float healthToAdd)
    {
        if (currentHealth == maxHealth && healthToAdd > 0) {return;}
        if (currentHealth + healthToAdd > maxHealth)
        {
            healthToAdd = maxHealth - currentHealth;
        }
        if (healthToAdd > 0)
        {
            currentHealth += healthToAdd;
            RedrawHealthGlobes();
            GameManager.Instance.SetCurrentHealthText(currentHealth);
        } 
        else
        {
            GetHurtCalculations(healthToAdd);
        }
    }

    public void SetMaxHealth(float healthToAdd)
    {
        maxHealth += healthToAdd;
        GameManager.Instance.SetMaxHealthText(maxHealth);

        if (maxHealth < currentHealth)
        {
            SetHealth(maxHealth - currentHealth);
        }
        RedrawHealthGlobes();
    }

     void AddMaxHealth(){
        int currentBackgroundCount = lifeBackGroundRow.transform.childCount;
        int backGroundChange = (int)maxHealth / 2 + (int)maxHealth % 2;
        for (int i = 0; i < backGroundChange - currentBackgroundCount; i++)
        {
            GameObject instantiatedLifeBackground = Instantiate(lifeBackGroundSprite, Vector3.up, Quaternion.identity);
            instantiatedLifeBackground.transform.SetParent(lifeBackGroundRow.transform);
            instantiatedLifeBackground.transform.localScale = new Vector3(.75f, .75f, .75f);
        }
    }

    void GetHurtCalculations(float hitAmount) {
        if (!playerInputHandler.Frozen() && !dead && hitAmount != 0 && !invulnerable)
        {
            AudioPlayer.Instance.PlayHurtSound();
            Instantiate(bloodParticles, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360f)));
            cameraShake.Play();

            if (currentHealth + hitAmount <= 0) {
                currentHealth = 0;
                
                if (GemBuffManager.Instance.GetRespawnOnDeath() == "half")
                {
                    SetHealth((float)Math.Floor(maxHealth / 2));
                    currentInvulnerabilityCounter = baseInvulnerabilityCounter;
                    invulnerable = true;
                    onDeath("half");
                }
                else if (GemBuffManager.Instance.GetRespawnOnDeath() == "full")
                {
                    SetHealth(maxHealth);
                    currentInvulnerabilityCounter = baseInvulnerabilityCounter;
                    invulnerable = true;
                    onDeath("full");
                }
                else
                {
                    StartCoroutine(Die());
                    GameObject.Instantiate(deathBloodParticles, transform.position, deathBloodParticles.transform.rotation);
                    GameObject.Instantiate(deathChunkParticles, transform.position, deathChunkParticles.transform.rotation);
                }
            }
            else
            {
                currentHealth += hitAmount;
            }
            RedrawHealthGlobes();
            GameManager.Instance.SetCurrentHealthText(currentHealth);
        }
    }

    public IEnumerator Die()
    {
        if (!playerInputHandler.Frozen())
        {
            playerInputHandler.FreezePlayer(true);
            dead = true;
            // AudioPlayer.Instance.PlayDeathSound();
            Time.timeScale = .6f;
            FadeOut(2.5f);
            Invoke("FadeIn", 2.5f);
            yield return new WaitForSeconds(3f);
            // GameManager.Instance.hud.animator.SetTrigger("coverScreen");
            // GameManager.Instance.hud.loadSceneName = SceneManager.GetActiveScene().name;
            Time.timeScale = 1f;
        }
    }

     void Fade(float endValue, TweenCallback onEnd)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = screenCanvas.DOFade(endValue, 1.2f);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn()
    {
        playerInputHandler.transform.position = respawnCoordinates;
        Fade(0f, () =>
        {
            screenCanvas.interactable = true;
            screenCanvas.blocksRaycasts = true;
        });
        Respawn();
    }

    void Respawn()
    {
        playerInputHandler.FreezePlayer(false);
        dead = false;
        SetHealth(maxHealth);
    }

    public void FadeOut(float duration)
    {
        Fade(1f, () =>
        {
            screenCanvas.interactable = false;
            screenCanvas.blocksRaycasts = false;
        });
    }

    public void ResetLevel()
    {
        playerInputHandler.FreezePlayer(true);
        dead = false;
        currentHealth = maxHealth;
    }

    public IEnumerator FreezeFrameEffect(float length = .01f)
    {
        Time.timeScale = .1f;
        yield return new WaitForSeconds(length);
        Time.timeScale = 1f;
    }
}

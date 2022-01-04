using UnityEngine;
using UnityEngine.UI;

public enum EnemyName {
    None,
    Slime,
    Skeleton,
    Trap
}

public class EnemyBase : MonoBehaviour
{
    enum State
    {
        Moving,
        Knockback,
        Dead
    }

    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float knockbackDuration;
    [SerializeField] float touchDamage;
    [SerializeField] float touchDamageWidth;
    [SerializeField] float touchDamageHeight;
    [SerializeField] float touchDamageCooldown;

    [SerializeField] Vector2 knockbackSpeed;

    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform touchDamageCheck;

    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask playerMask;

    [SerializeField] GameObject bloodParticles;
    [SerializeField] GameObject deathBloodParticles;
    [SerializeField] GameObject deathChunkParticles;

    GameObject aliveObject;
    State currentState;
    Rigidbody2D rb;
    Vector2 movement;
    Vector2 touchDamageBotLeft;
    Vector2 touchDamageTopRight;
    Animator animator;

    bool groundDetected;
    bool wallDetected;

    float knockbackStartTime;
    float currentHealth;
    float lastTouchDamageTime;
    float[] attackDetails = new float[2];

    int damageDirection;
    int facingDir;

    void Awake()
    {
        aliveObject = GetComponentInChildren<SpriteRenderer>().gameObject;   
        rb = aliveObject.GetComponent<Rigidbody2D>(); 
        animator = aliveObject.GetComponent<Animator>();
    }
    
    void Start()
    {
        facingDir = 1;
        currentHealth = maxHealth;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }    
    }

    //////////////////////////////// Moving STATE ////////////////////////////////////

    void EnterMovingState()
    {

    }

    void UpdateMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundMask);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, groundMask);

        CheckTouchDamage();

        if (!groundDetected || wallDetected)
        {
            Flip();
        }
        else
        {
            movement.Set(moveSpeed * facingDir, rb.velocity.y);
            rb.velocity = movement;
        }
    }

    void ExitMovingState()
    {

    }

    //////////////////////////////// KNOCKBACK STATE ////////////////////////////////////

    void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        rb.velocity = movement;
        animator.SetBool("knockback", true);
    }

    void UpdateKnockbackState()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }

    void ExitKnockbackState()
    {
        animator.SetBool("knockback", false);
    }

    //////////////////////////////// DEAD STATE ////////////////////////////////////

    void EnterDeadState()
    {
        Instantiate(deathChunkParticles, aliveObject.transform.position, deathChunkParticles.transform.rotation);
        Instantiate(deathBloodParticles, aliveObject.transform.position, deathBloodParticles.transform.rotation);
        Destroy(gameObject);
    }

    void UpdateDeadState()
    {

    }

    void ExitDeadState()
    {
        
    }

    //////////////////////////////// OTHER FUNCTIONS ////////////////////////////////////

    void SwitchState(State state)
    {
        switch(currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
        }

        switch(state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }

    void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];

        Instantiate(bloodParticles, aliveObject.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360f)));

        damageDirection = attackDetails[1] > aliveObject.transform.position.x ? -1 : 1;

        if (currentHealth > 0f)
        {
            SwitchState(State.Knockback);
        }
        else if (currentHealth <= 0f)
        {
            SwitchState(State.Dead);
        }
    }

    void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, playerMask);

            if (hit)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = aliveObject.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

    void Flip()
    {
        facingDir *= -1;
        aliveObject.transform.Rotate(0, 180f, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }

    // [Header ("Reference")]
    // [SerializeField] ParticleSystem blood;
    // [System.NonSerialized] public AudioSource audioSource;
    // [SerializeField] EnemyName myName;
    // public Animator animator;
    // CameraShake cameraShake;
    // public event Action OnEnemyDeath; 
    // Image healthBar;

    // [Header ("Properties")]
    // [SerializeField] private GameObject deathParticles;
    // [SerializeField] float baseHealth = 3;
    // [SerializeField] float currentHealth;
    // [SerializeField] bool requirePoundAttack;
    // [SerializeField] AudioClip hitSound;
    // [SerializeField] float bloodOffsetY = .3f;

    // bool inKillArea = false;
    // bool dead = false;

    // void Awake()
    // {
    //     cameraShake = FindObjectOfType<CameraShake>();
    //     healthBar = GetComponentInChildren<Canvas>().transform.GetChild(1).GetComponent<Image>();
    //     // audioSource = GetComponent<AudioSource>();
    //     // animatorFunctions = GetComponent<AnimatorFunctions>();
    // }

    // private void Start() {
    //     onEnemyDeath += TestFunction;
    // }

    // void TestFunction()
    // {
    //     Debug.Log("enemy death event call bug");
    // }

    // private void Start() {
    //     healthBar.fillAmount = 1f;
    //     currentHealth = baseHealth;
    // }

    // void Update()
    // {
    //     if (currentHealth <= 0 && !dead)
    //     {
    //         dead = true;
    //         if (inKillArea)
    //         {
    //             GameManager.Instance.AddToKillCount(myName);
    //         }
    //         OnEnemyDeath();
    //         Die();
    //     }
    // }

    // private void OnTriggerStay2D(Collider2D other) {
    //     if (other.CompareTag("KillArea"))
    //     {
    //         inKillArea = true;
    //     }
    // }

    // public void GetHurt(int launchDirection, float hitPower)
    // {
    //     if (GetComponent<GroundedEnemy>() != null)
    //     {
    //         cameraShake.Play();
    //         currentHealth -= hitPower;
    //         healthBar.fillAmount = currentHealth / baseHealth;
    //         animator.SetTrigger("hurt");

            // audioSource.pitch = (1);
            // audioSource.PlayOneShot(hitSound);

            // if (GetComponent<GroundedEnemy>() != null)
            // {
            //     GroundedEnemy groundedEnemy = GetComponent<GroundedEnemy>();
                // groundedEnemy.launch = launchDirection * groundedEnemy.hurtLaunchPower;
                // groundedEnemy.velocity.y = groundedEnemy.hurtLaunchPower;
                // groundedEnemy.directionSmooth = launchDirection;
                // groundedEnemy.direction = groundedEnemy.directionSmooth;
            // }

            // if (GetComponent<Flyer>() != null)
            // {
            //     Flyer flyer = GetComponent<Flyer>();
            //     flyer.speedEased.x = launchDirection * 5;
            //     flyer.speedEased.y = 4;
            //     flyer.speed.x = flyer.speedEased.x;
            //     flyer.speed.y = flyer.speedEased.y;
            // }

            // PlayerBase.Instance.FreezeFrameEffect();   
    //     }
    // }

    // public void Die()
    // {
    //     GameObject bloodInstance = Instantiate(blood.gameObject, new Vector2(transform.position.x, transform.position.y + bloodOffsetY), Quaternion.identity, null) as GameObject;
    //     // int xScale = transform.position.x < PlayerBase.Instance.transform.position.x ? 1 : -1;
    //     // bloodInstance.transform.localScale = new Vector2(xScale, 1);
    //     AudioPlayer.Instance.PlayBloodSound();
    //     Destroy(bloodInstance, blood.main.startLifetimeMultiplier);

    //     cameraShake.Play();
    //     currentHealth = 0;
    //     // deathParticles.SetActive(true);
    //     // deathParticles.transform.parent = transform.parent;
    //     // instantiator.InstantiateObjects();
    //     Time.timeScale = 1f;
    //     Destroy(gameObject);
    // }
}
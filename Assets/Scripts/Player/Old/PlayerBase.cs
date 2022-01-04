using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System;

public class PlayerBase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float movementForceInAir;
    [SerializeField] float airDragMultiplier;
    [SerializeField] float jumpHeightMultiplier = .5f;
    [SerializeField] float knockbackDuration;
    [SerializeField] Vector2 knockbackSpeed;

    [Header("Walljumping")]
    [SerializeField] float wallSlideSpeed = .3f;
    [SerializeField] float wallJumpForce;
    [SerializeField] float wallHopForce;
    [SerializeField] float jumpTimerSet = .15f;
    [SerializeField] float turnTimerSet = .1f;
    [SerializeField] Vector2 wallHopDirection;
    [SerializeField] Vector2 wallJumpDirection;

    [Header("Ledge climbing")]
    [SerializeField] float ledgeClimbXOffset1 = 0f;
    [SerializeField] float ledgeClimbYOffset1 = 0f;
    [SerializeField] float ledgeClimbXOffset2 = 0f;
    [SerializeField] float ledgeClimbYOffset2 = 0f;

    [Header("Dashing")]
    [SerializeField] float dashTime;
    [SerializeField] float dashSpeed;
    [SerializeField] float distanceBetweenAfterImages;
    [SerializeField] float dashCooldown;

    [Header("References")]
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform ledgeCheck;
    [SerializeField] LayerMask groundMask;

    public int availableJumps = 1;

    Animator animator;
    Rigidbody2D rb;
    IEnumerator footStepsCoroutine;

    Vector2 moveInput;
    Vector2 currentDir;
    Vector2 ledgePosBot;
    Vector2 ledgePos1;
    Vector2 ledgePos2;

    int jumpsLeft;
    int facingDir = 1;

    float jumpTimer;
    float turnTimer;
    float dashTimeLeft;
    float lastAfterImageXPos;
    float lastDash = -100f;
    float knockbackStartTime;

    bool isTryingToJump;
    bool isFacingRight = true;
    bool isWalking;
    bool isGrounded;
    bool canNormalJump;
    bool canWallJump;
    bool isTouchingWall;
    bool isWallSliding;
    bool checkJumpMultiplier;
    bool canMove;
    bool canFlip;
    bool isTouchingLedge;
    bool canClimbLedge = false;
    bool ledgeDetected;
    bool isDashing;
    bool knockback;
    bool playingFootsteps;

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // [Header ("Reference")]
    // // [SerializeField] ParticleSystem deathParticles;
    // [SerializeField] ParticleSystem jumpParticles;
    // [SerializeField] GameObject pauseMenu;
    // [SerializeField] GameObject lifeRow;
    // [SerializeField] GameObject lifeBackGroundRow;
    // [SerializeField] GameObject lifeBackGroundSprite;
    // [SerializeField] GameObject lifeOrbUI;
    // [SerializeField] GameObject lifeOrbHalfUI;
    // [SerializeField] CanvasGroup screenCanvas;
    // // [SerializeField] CameraEffects cameraEffects;

    // // AnimatorFunctions animatorFunctions;
    // CapsuleCollider2D capsuleCollider;
    // CameraShake cameraShake;
    
    static PlayerBase instance;
    public static PlayerBase Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<PlayerBase>();
            return instance;
        }
    }

    // [Header("Properties")]
    // [SerializeField] string[] cheatItems;
    // [SerializeField] float hangTime = .2f;
    // [SerializeField] Vector2 hurtLaunchPower;
    // [SerializeField] float launchRecovery;
    // [SerializeField] Vector2 respawnCoordinates;

    // public string groundType = "grass";
    // public RaycastHit2D ground; 
    // public RaycastHit2D leftWallUp;
    // public RaycastHit2D rightWallUp;
    // public RaycastHit2D leftWallBottom;
    // public RaycastHit2D rightWallBottom; 
    // public bool attacking = false;
    // public bool dead = false;
    // public bool frozen = false;
    // public float maxSpeed = 7;
    // public float jumpPower = 17;
    // public float recoveryTime = .5f;
    // float lastFacingIndex;
    // float baseInvulnerabilityCounter = 2f;
    // float currentInvulnerabilityCounter = 2f;
    // [System.NonSerialized] public float recoveryCounter;

    // Vector3 originalLocalScale;
    // bool recovering = false;
    // float hangTimeCounter;
    // float launch;
    // bool launched;
    // bool jumping;
    // bool playingFootsteps = false;
    // bool invulnerable = false;

    // [Header ("Health")]
    // public float currentHealth;
    // public float maxHealth;

    // public event Action<string> onDeath;
    // IEnumerator footStepsCoroutine;
    // Tween fadeTween;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // cameraShake = FindObjectOfType<CameraShake>();
        // capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        jumpsLeft = availableJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        // groundMask = LayerMask.GetMask("Ground");    
    }

    // void Start()
    // {

        
    //     /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //     // Cursor.visible = false;
    //     currentHealth = maxHealth;
        
    //     for (int i = 0; i < ((int)currentHealth / 2); i++)
    //     {
    //         GameObject instantiatedOrb = Instantiate(lifeOrbUI, Vector3.up, Quaternion.identity);
    //         instantiatedOrb.transform.SetParent(lifeRow.transform);
    //         instantiatedOrb.transform.localScale = new Vector3(.6f, .6f, .6f);

    //         GameObject instantiatedLifeBackground = Instantiate(lifeBackGroundSprite, Vector3.up, Quaternion.identity);
    //         instantiatedLifeBackground.transform.SetParent(lifeBackGroundRow.transform);
    //         instantiatedLifeBackground.transform.localScale = new Vector3(.75f, .75f, .75f);
    //     }
    //     if ((int)currentHealth % 2 == 1)
    //     {
    //         GameObject instantiatedOrb = Instantiate(lifeOrbHalfUI, Vector3.up, Quaternion.identity);
    //         instantiatedOrb.transform.SetParent(lifeRow.transform);
    //         instantiatedOrb.transform.localScale = new Vector3(.6f, .6f, .6f);

    //         GameObject instantiatedLifeBackground = Instantiate(lifeBackGroundSprite, Vector3.up, Quaternion.identity);
    //         instantiatedLifeBackground.transform.SetParent(lifeBackGroundRow.transform);
    //         instantiatedLifeBackground.transform.localScale = new Vector3(.75f, .75f, .75f);
    //     }
    //     originalLocalScale = graphic.transform.localScale;
    // }

    void Update()
    {
        CheckInput();
        CheckDirection();
        UpdateAnimations();
        CheckJumpAvailability();
        CheckIfWallSliding();
        CheckJump();
        CheckLedgeClimb();
        CheckDash();
        CheckKnockback();
    }

    void FixedUpdate()
    {
        Movement();
        CheckForWallAndGround();
    }

    public bool GetDashStatus()
    {
        return isDashing;
    }

    public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

    void CheckKnockback()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public int GetPlayerFacing()
    {
        return facingDir;
    }

    void UpdateAnimations()
    {
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("isWallSliding", isWallSliding);
    }

    void CheckForWallAndGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .025f, groundMask);
        currentDir = Mathf.Sign(transform.localScale.x) < 0 ? -transform.right : transform.right;
        isTouchingWall = Physics2D.Raycast(wallCheck.position, currentDir, .35f, groundMask);
        isTouchingLedge = Physics2D.Raycast(ledgeCheck.position, currentDir, .35f, groundMask);

        if (isTouchingWall && !isTouchingLedge && !ledgeDetected)
        {
            ledgeDetected = true;
            ledgePosBot = wallCheck.position;
        }
    }

    void CheckIfWallSliding()
    {
        isWallSliding = isTouchingWall && rb.velocity.y < 0 && moveInput.x == facingDir && !canClimbLedge ? true : false;
    }

    void OnMove(InputValue value) {
        // if (dead)
        // {
        //     moveInput.x = 0;
        //     return;
        // }
        moveInput = value.Get<Vector2>();
        // lastFacingIndex = moveInput.x == 0 ? lastFacingIndex : moveInput.x;
    }

    void CheckInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (isGrounded || (jumpsLeft > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isTryingToJump = true;
            }
        }

        if (moveInput.x != 0 && isTouchingWall)
        {
            if (!isGrounded && moveInput.x != facingDir)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Keyboard.current.spaceKey.isPressed)
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpHeightMultiplier);
        }

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
        {
            if (Time.time >= (lastDash + dashCooldown))
            {
                AudioPlayer.Instance.PlayDash();
                AttemptToDash();
            }
        }
    }

    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

    void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDir, 0);
                dashTimeLeft -= Time.deltaTime;

                if (Math.Abs(transform.position.x - lastAfterImageXPos) > distanceBetweenAfterImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastAfterImageXPos = transform.position.x;
                }
            }

            if (dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
        }
    }

    void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastAfterImageXPos = transform.position.x;
    }

    public void FinishLedgeClimb()
    {
        canClimbLedge = false;
        transform.position = ledgePos2;
        canMove = true;
        canFlip = true;
        ledgeDetected = false;
        animator.SetBool("canClimbLedge", canClimbLedge);
    }

    void CheckLedgeClimb()
    {
        if (ledgeDetected && !canClimbLedge)
        {
            canClimbLedge = true;
            if (isFacingRight)
            {
                ledgePos1 = new Vector2((float)Math.Floor(ledgePosBot.x + .35f) - ledgeClimbXOffset1, (float)Math.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2((float)Math.Floor(ledgePosBot.x + .35f) + ledgeClimbXOffset2, (float)Math.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }
            else
            {
                ledgePos1 = new Vector2((float)Math.Ceiling(ledgePosBot.x - .35f) + ledgeClimbXOffset1, (float)Math.Floor(ledgePosBot.y) + ledgeClimbYOffset1);
                ledgePos2 = new Vector2((float)Math.Ceiling(ledgePosBot.x - .35f) - ledgeClimbXOffset2, (float)Math.Floor(ledgePosBot.y) + ledgeClimbYOffset2);
            }

            canMove = false;
            canFlip = false;

            animator.SetBool("canClimbLedge", canClimbLedge);
        }

        if (canClimbLedge)
        {
            transform.position = ledgePos1;
        }
    }

    void CheckJumpAvailability()
    {
        if (isGrounded || isWallSliding)
        {
            jumpsLeft = availableJumps;
        }

        canWallJump = isTouchingWall ? true : false;
        canNormalJump = jumpsLeft <= 0 ? false : true;
    }

    void CheckDirection()
    {
        if (isFacingRight && moveInput.x < 0)
        {
            Flip();
        }
        else if (!isFacingRight && moveInput.x > 0)
        {
            Flip();
        }

        isWalking = Mathf.Abs(rb.velocity.x) >= 0.01f ? true : false;
    }

    void CheckJump()
    {
        if (jumpTimer > 0)
        {
            if (!isGrounded && isTouchingWall && moveInput.x != 0 && moveInput.x != facingDir)
            {
                WallJump();
            }
            else if (!isGrounded && isTouchingWall && moveInput.x == 0)
            {
                WallHop();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }
        
        if (isTryingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }
    }

    void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            Invoke("DecreaseJumpsLeft", .1f);
            jumpTimer = 0;
            isTryingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    void WallHop()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            isWallSliding = false;
            jumpsLeft = availableJumps;
            Invoke("DecreaseJumpsLeft", .1f);
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDir, wallHopForce * wallHopDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isTryingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
        }
    }

    void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            isWallSliding = false;
            jumpsLeft = availableJumps;
            Invoke("DecreaseJumpsLeft", .1f);
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * moveInput.x, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isTryingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
        }
    }

    void DecreaseJumpsLeft()
    {
        jumpsLeft--;
    }

    void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDir *= -1;
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void Movement()
    {
        if (!isGrounded && !isWallSliding && moveInput.x == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove && !knockback)
        {
            rb.velocity = new Vector2(moveSpeed * moveInput.x, rb.velocity.y);
        }
        // else if (!isGrounded && !isWallSliding && moveInput.x != 0)
        // {
        //     Vector2 forceToAdd = new Vector2(movementForceInAir * moveInput.x, 0);
        //     rb.AddForce(forceToAdd);

        //     if (Math.Abs(rb.velocity.x) > moveSpeed)
        //     {
        //         rb.velocity = new Vector2(moveSpeed * moveInput.x, rb.velocity.y);
        //     }
        // }
        // else if (!isGrounded && !isWallSliding && moveInput.x == 0)
        // {
        //     rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        // }

        if (isWallSliding)
        {
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        if (moveInput.x != 0 && !playingFootsteps && isGrounded)
        {
            playingFootsteps = true;
            footStepsCoroutine = AudioPlayer.Instance.PlayFootsteps();
            StartCoroutine(footStepsCoroutine);
        }
        else if (moveInput.x == 0 && playingFootsteps)
        {
            playingFootsteps = false;
            StopCoroutine(footStepsCoroutine);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, .025f);
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + Math.Sign(currentDir.x) * .35f, wallCheck.position.y));
    }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // public void Jump(float jumpMultiplier)
    // {
        // if (velocity.y != jumpPower)
        // {
        //     velocity.y = jumpPower * jumpMultiplier;
        //     AudioPlayer.Instance.PlayJumpSound();
        //     jumping = true;
        // }
    // }

    // public void SetJumpPower(float power)
    // {
    //     jumpPower += power;
    // }

    // public void LandEffect()
    // {
    //     if (jumping)
    //     {
    //         jumpParticles.Emit(30);
    //         jumping = false;
    //     }
    // }

    // public void SetHealth(float healthToAdd)
    // {
    //     if (currentHealth == maxHealth) {return;}
    //     if (currentHealth + healthToAdd > maxHealth)
    //     {
    //         healthToAdd = maxHealth - currentHealth;
    //     }
    //     if (healthToAdd > 0)
    //     {
    //         currentHealth += healthToAdd;
    //         foreach (Transform lifeOrb in lifeRow.transform)
    //         {
    //             Destroy(lifeOrb.gameObject);
    //         }
    //         for (int i = 0; i < (int)currentHealth / 2; i++)
    //         {
    //             GameObject instantiatedOrb = Instantiate(lifeOrbUI, Vector3.up, Quaternion.identity);
    //             instantiatedOrb.transform.SetParent(lifeRow.transform);
    //             instantiatedOrb.transform.localScale = new Vector3(.6f, .6f, .6f);
    //         }
    //         if ((int)currentHealth % 2 == 1)
    //         {
    //             GameObject instantiatedOrb = Instantiate(lifeOrbHalfUI, Vector3.up, Quaternion.identity);
    //             instantiatedOrb.transform.SetParent(lifeRow.transform);
    //             instantiatedOrb.transform.localScale = new Vector3(.6f, .6f, .6f);
    //         }
    //         GameManager.Instance.SetCurrentHealthText(currentHealth);
    //     } 
    //     else
    //     {
    //         GetHurt(healthToAdd);
    //     }
    // }

    // public void SetMaxHealth(float healthToAdd)
    // {
    //     maxHealth += healthToAdd;
    //     GameManager.Instance.SetMaxHealthText(maxHealth);
    //     int currentBackgroundCount = lifeBackGroundRow.transform.childCount;
    //     int backGroundChange = (int)maxHealth / 2 + (int)maxHealth % 2;
    //     if (healthToAdd > 0)
    //     {
    //         Invoke("AddMaxHealth", .01f);
    //     } 
    //     else
    //     {
    //         for (int i = 0; i < currentBackgroundCount - backGroundChange; i++)
    //         {
    //             Destroy(lifeBackGroundRow.transform.GetChild(lifeBackGroundRow.transform.childCount - 1).gameObject);
    //         }
    //         if (maxHealth < currentHealth)
    //         {
    //             SetHealth(maxHealth - currentHealth);
    //         }
    //     }
    // }

    // public void Freeze(bool freeze)
    // {
    //     if (freeze)
    //     {
    //         animator.SetInteger("moveDirection", 0);
    //         animator.SetBool("isGrounded", true);
    //         animator.SetFloat("velocityX", 0f);
    //         animator.SetFloat("velocityY", 0f);
    //         GetComponent<PhysicsObject>().targetVelocity = Vector2.zero;
    //     }

    //     frozen = freeze;
    //     attacking = false;
    //     launch = 0;
    // }

    // public void GetHurt(float hitAmount)
    // {
    //     GetHurtCalculations(5, hitAmount);
    // }

    // public void GetHurt(int hurtDirection, float hitAmount)
    // {
    //     GetHurtCalculations(hurtDirection, hitAmount);
    // }

    // void GetHurtCalculations(int hurtDirection, float hitAmount) {
    //     if (!frozen && !recovering && !dead && hitAmount != 0 && !invulnerable)
    //     {
    //         // HurtEffect();
    //         AudioPlayer.Instance.PlayHurtSound();
    //         cameraShake.Play();
    //         recoveryCounter = 0;
    //         for (int i = 0; i < Mathf.Abs(hitAmount); i++)
    //         {
    //             if (currentHealth > 0)
    //             {
    //                 if (currentHealth - 1 == 0) {
    //                     currentHealth -= 1;
    //                     Destroy(lifeRow.transform.GetChild(0).gameObject);
    //                     if (GemBuffManager.Instance.GetRespawnOnDeath() == "half")
    //                     {
    //                         SetHealth((float)Math.Floor(maxHealth / 2));
    //                         currentInvulnerabilityCounter = baseInvulnerabilityCounter;
    //                         invulnerable = true;
    //                         onDeath("half");
    //                     }
    //                     else if (GemBuffManager.Instance.GetRespawnOnDeath() == "full")
    //                     {
    //                         SetHealth(maxHealth);
    //                         currentInvulnerabilityCounter = baseInvulnerabilityCounter;
    //                         invulnerable = true;
    //                         onDeath("full");
    //                     }
    //                     else
    //                     {
    //                         // StartCoroutine(Die());
    //                     }
    //                 }
    //                 else
    //                 {
    //                     currentHealth -= 1;
    //                     if (lifeRow.transform.GetChild(lifeRow.transform.childCount - 1).childCount == 1)
    //                     {
    //                         Destroy(lifeRow.transform.GetChild(lifeRow.transform.childCount - 1).GetChild(0).gameObject);
    //                     }
    //                     else
    //                     {
    //                         Destroy(lifeRow.transform.GetChild(lifeRow.transform.childCount - 1).gameObject);
    //                     }
    //                 } 
    //                 // } else if (health % 2 == 1) {
    //                 //     Destroy(lifeRow.transform.GetChild((int)(health/2)).gameObject);
    //                 // } else {
    //                 //     Destroy(lifeRow.transform.GetChild((int)(health/2)-1).GetChild(0).gameObject);
    //                 // }
    //             }
    //         }
    //         GameManager.Instance.SetCurrentHealthText(currentHealth);
    //         animator.SetTrigger("hurt");
    //         if (hurtDirection != 5) {
    //             launched = true;
    //             // velocity.y = hurtLaunchPower.y;
    //             launch = hurtDirection * hurtLaunchPower.x;
    //             Invoke("ResetLaunch", .3f);
    //         }
    //     }
    // } 

    // public IEnumerator FreezeFrameEffect(float length = .01f)
    // {
    //     Time.timeScale = .1f;
    //     yield return new WaitForSeconds(length);
    //     Time.timeScale = 1f;
    // }

//     void Update()
//     {
//         if (invulnerable && currentInvulnerabilityCounter > 0)
//         {
//             currentInvulnerabilityCounter -= Time.deltaTime;
//             if (currentInvulnerabilityCounter <= 0) { invulnerable = false; }
//         }

//         ComputeVelocity();
//         if(recoveryCounter <= recoveryTime)
//         {
//             recoveryCounter += Time.deltaTime;
//             recovering = true;
//         }
//         else
//         {
//             recovering = false;
//         }
//     }

    // public Vector2 GetMoveInput()
    // {
    //     return moveInput;
    // }


//     void AddMaxHealth(){
//         int currentBackgroundCount = lifeBackGroundRow.transform.childCount;
//         int backGroundChange = (int)maxHealth / 2 + (int)maxHealth % 2;
//         for (int i = 0; i < backGroundChange - currentBackgroundCount; i++)
//         {
//             GameObject instantiatedLifeBackground = Instantiate(lifeBackGroundSprite, Vector3.up, Quaternion.identity);
//             instantiatedLifeBackground.transform.SetParent(lifeBackGroundRow.transform);
//             instantiatedLifeBackground.transform.localScale = new Vector3(.75f, .75f, .75f);
//         }
//     }

//     private void OnDrawGizmos() {
//         Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y + .3f), new Vector2(transform.position.x, transform.position.y + .3f) + Vector2.left * .3f);
//         Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y - .9f), new Vector2(transform.position.x, transform.position.y - .9f) + Vector2.left * .3f);
//         Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y + .3f), new Vector2(transform.position.x, transform.position.y + .3f) + Vector2.right * .3f);
//         Gizmos.DrawLine(new Vector2(transform.position.x, transform.position.y - .9f), new Vector2(transform.position.x, transform.position.y - .9f) + Vector2.right * .3f);
//     }

//     protected void ComputeVelocity()
//     {
//         Vector2 move = Vector2.zero;
//         ground = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -Vector2.up);
//         leftWallUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + .3f), Vector2.left, .3f, LayerMask.GetMask("Ground"));
//         leftWallBottom = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - .9f), Vector2.left, .3f, LayerMask.GetMask("Ground"));
//         rightWallUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + .3f), Vector2.right, .3f, LayerMask.GetMask("Ground"));
//         rightWallBottom = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - .9f), Vector2.right, .3f, LayerMask.GetMask("Ground"));

//         launch += (0 - launch) * Time.deltaTime * launchRecovery;

//         if (Keyboard.current.escapeKey.wasPressedThisFrame)
//         {
//             pauseMenu.SetActive(true);
//         }

//         if (!frozen)
//         {
//             move.x = moveInput.x + launch;
                
//             if (Keyboard.current.spaceKey.wasPressedThisFrame && animator.GetBool("isGrounded") && !jumping)
//             {
//                 Jump(1f);
//             }

//             if (Keyboard.current.leftShiftKey.wasPressedThisFrame && animator.GetBool("isGrounded") && !animator.GetBool("isRolling"))
//             {
//                 animator.SetBool("isRolling", true);
//                 graphic.transform.localPosition = new Vector2(graphic.transform.localPosition.x, graphic.transform.localPosition.y - .42f);
//                 launch = lastFacingIndex * 3f;
//                 launched = true;
//                 Invoke("ResetLaunch", .26f);
//             }

//             if (( (leftWallUp && leftWallBottom) || (rightWallUp && rightWallBottom)) && velocity.y < 0 && !grounded) {
//                 lastFacingIndex = leftWallUp ? -1 : 1;
//                 animator.SetBool("isWallSliding", true);
//                 velocity.y = -.1f;
//                 float launchPowerX = -lastFacingIndex * hurtLaunchPower.x * 2.5f;
//                 if ((leftWallUp && Keyboard.current.leftArrowKey.isPressed) || (rightWallUp && Keyboard.current.rightArrowKey.isPressed)) {
//                     launchPowerX = -lastFacingIndex * hurtLaunchPower.x * 1.4f;
//                 }
//                 if (Keyboard.current.spaceKey.wasPressedThisFrame)
//                 {
//                     lastFacingIndex *= -1;
//                     velocity.y = hurtLaunchPower.y * 2.36f;
//                     launch = launchPowerX;
//                     launched = true;
//                     Invoke("ResetLaunch", .26f);
//                 }
//             } 
//             else
//             {
//                 animator.SetBool("isWallSliding", false);
//             }

//             if (!grounded && (rightWallUp.point == new Vector2(0,0)) && rightWallBottom.point != new Vector2(0,0) && jumping)
//             {
//                 animator.SetTrigger("cornerGrab");
//                 capsuleCollider.enabled = false;
//                 // rb.position = new Vector2(transform.position.x + .5f, Mathf.Floor(rightWallBottom.point.y + 1));
//                 transform.position = new Vector2(transform.position.x + .5f, Mathf.Floor(rightWallBottom.point.y + 1));
//                 capsuleCollider.enabled = true;
//             }

//             if (!grounded && (leftWallUp.point == new Vector2(0,0)) && leftWallBottom.point != new Vector2(0,0) && jumping)
//             {
//                 animator.SetTrigger("cornerGrab");
//                 capsuleCollider.enabled = false;
//                 // rb.position = new Vector2(transform.position.x - .5f, Mathf.Floor(leftWallBottom.point.y + 1));
//                 transform.position = new Vector2(transform.position.x + .5f, Mathf.Floor(leftWallBottom.point.y + 1));
//                 capsuleCollider.enabled = true;
//             }

//             if (!launched) {
//                 if (moveInput.x > 0.1f)
//                 {
//                     graphic.transform.localScale = new Vector3(originalLocalScale.x, originalLocalScale.y, originalLocalScale.z);
//                 }
//                 else if (moveInput.x < -0.1f)
//                 {
//                     graphic.transform.localScale = new Vector3(-originalLocalScale.x, originalLocalScale.y, originalLocalScale.z);
//                 }
//             }

//             if (!grounded)
//             {
//                 if (hangTimeCounter < hangTime && !jumping)
//                 {
//                     hangTimeCounter += Time.deltaTime;
//                 }
//                 else
//                 {
//                     animator.SetBool("isGrounded", false);
//                 }
//             }
//             else
//             {
//                 hangTimeCounter = 0;
//                 animator.SetBool("isGrounded", true);
//             }

//             animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
//             animator.SetFloat("velocityY", velocity.y);
//             animator.SetInteger("attackDirectionY", (int)moveInput.y);
//             if (animator.GetBool("isWallSliding") || dead)
//             {
//                 animator.SetInteger("moveDirection", 0);
//             }
//             else
//             {
//                 animator.SetInteger("moveDirection", (int)moveInput.x);
//             }
//             targetVelocity = move * maxSpeed;
//         }
//         else
//         {
//             launch = 0;
//         }

//         if (move.x != 0 && !playingFootsteps)
//         {
//             playingFootsteps = true;
//             footStepsCoroutine = AudioPlayer.Instance.PlayFootsteps();
//             StartCoroutine(footStepsCoroutine);
//         }
//         else if (move.x == 0 && playingFootsteps)
//         {
//             playingFootsteps = false;
//             StopCoroutine(footStepsCoroutine);
//         }
//     }

//     void ResetLaunch()
//     {
//         if (animator.GetBool("isRolling"))
//         {
//             graphic.transform.localPosition = new Vector2(graphic.transform.localPosition.x, graphic.transform.localPosition.y + .42f);
//             animator.SetBool("isRolling", false);
//         } 
//         launched = false;
//         launch = 0;
//     }

//     void HurtEffect()
//     {
//         StartCoroutine(FreezeFrameEffect());
//     }

//     public IEnumerator Die()
//     {
//         if (!frozen)
//         {
//             Freeze(true);
//             dead = true;
//             // deathParticles.Emit(10);
//             // AudioPlayer.Instance.PlayDeathSound();
//             animator.SetTrigger("death");
//             Time.timeScale = .6f;
//             FadeOut(2.2f);
//             Invoke("FadeIn", 2.2f);
//             yield return new WaitForSeconds(5f);
//             // GameManager.Instance.hud.animator.SetTrigger("coverScreen");
//             // GameManager.Instance.hud.loadSceneName = SceneManager.GetActiveScene().name;
//             Time.timeScale = 1f;
//         }
//     }

//     void Fade(float endValue, TweenCallback onEnd)
//     {
//         if (fadeTween != null)
//         {
//             fadeTween.Kill(false);
//         }

//         fadeTween = screenCanvas.DOFade(endValue, 1.2f);
//         fadeTween.onComplete += onEnd;
//     }

//     public void FadeIn()
//     {
//         PlayerBase.Instance.transform.position = respawnCoordinates;
//         Fade(0f, () =>
//         {
//             screenCanvas.interactable = true;
//             screenCanvas.blocksRaycasts = true;
//         });
//         Respawn();
//     }

//     void Respawn()
//     {
//         Freeze(false);
//         animator.SetTrigger("respawn");
//         dead = false;
//         SetHealth(maxHealth);
//     }

//     public void FadeOut(float duration)
//     {
//         Fade(1f, () =>
//         {
//             screenCanvas.interactable = false;
//             screenCanvas.blocksRaycasts = false;
//         });
//     }

//     public void ResetLevel()
//     {
//         Freeze(true);
//         dead = false;
//         currentHealth = maxHealth;
//     }
}
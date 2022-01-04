using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemy : PhysicsObject
{
    [Header ("Reference")]
    [SerializeField] GameObject graphic;
    public EnemyBase enemyBase;

    [Header ("Properties")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] EnemyType enemyType;
    [SerializeField] Vector2 rayCastSize = new Vector2(1.5f, 1);
    [SerializeField] Vector2 rayCastOffset;
    [SerializeField] bool followPlayer;
    // [SerializeField] bool flipWhenTurning = true;
    [SerializeField] bool jumping;
    [SerializeField] float maxSpeedDeviation;
    [SerializeField] bool neverStopFollowing = false;
    [SerializeField] bool sitStillWhenNotFollowing = false;
    [SerializeField] enum EnemyType { Bug, Zombie };
    [SerializeField] float xScale;
   
    [System.NonSerialized] public float direction = 1;
    [System.NonSerialized] public float directionSmooth = 1;
    [System.NonSerialized] public bool jump = false;
    [System.NonSerialized] public float launch = 1;

    RaycastHit2D ground;
    RaycastHit2D rightWall;
    RaycastHit2D leftWall;
    RaycastHit2D rightLedge;
    RaycastHit2D leftLedge;
    Vector3 origScale;
    Vector2 rayCastSizeOrig;
    float distanceFromPlayer;

    float sitStillMultiplier = 1; 

    public float attentionRange;
    public float changeDirectionEase = 1;
    public float hurtLaunchPower = 10;
    public float jumpPower = 7;
    public float maxSpeed = 7;
    public bool facingLeft;

    [Header("Sounds")]
    public AudioClip jumpSound;
    public AudioClip stepSound;
    
    void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
    }

    void Start()
    {
        origScale = transform.localScale;
        rayCastSizeOrig = rayCastSize;
        maxSpeed -= Random.Range(0, maxSpeedDeviation);
        launch = 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attentionRange);
    }

    void Update()
    {
        ComputeVelocity();
    }

    protected void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        // if (!PlayerBase.Instance.frozen)
        // {
        //     // distanceFromPlayer = new Vector2 (PlayerBase.Instance.gameObject.transform.position.x - transform.position.x, PlayerBase.Instance.gameObject.transform.position.y - transform.position.y);
        //     distanceFromPlayer = Vector2.Distance(transform.position, PlayerBase.Instance.gameObject.transform.position);
        //     directionSmooth += (direction - directionSmooth) * Time.deltaTime * changeDirectionEase;
        //     move.x = (1 * directionSmooth) + launch;
        //     launch += (0 - launch) * Time.deltaTime;

        //     // if (!enemyBase.recovering)
        //     // {
        //         if (move.x > 0.01f)
        //         {
        //             if (graphic.transform.localScale.x == -xScale)
        //             {
        //                 facingLeft = false;
        //                 graphic.transform.localScale = new Vector3(xScale, graphic.transform.localScale.y, graphic.transform.localScale.z);
        //             }
        //         }
        //         else if (move.x < -0.01f)
        //         {
        //             if (graphic.transform.localScale.x == xScale)
        //             {
        //                 facingLeft = true;
        //                 graphic.transform.localScale = new Vector3(-xScale, graphic.transform.localScale.y, graphic.transform.localScale.z);
        //             }
        //         }

        //         ground = Physics2D.Raycast(transform.position, -Vector2.up / 2);
        //         Debug.DrawRay(transform.position, -Vector2.up / 2, Color.green);
            
        //         if (enemyType == EnemyType.Zombie)
        //         {
        //             if ((Mathf.Abs(distanceFromPlayer) < attentionRange))
        //             {
        //                 followPlayer = true;
        //                 sitStillMultiplier = 1;
        //                 if(Mathf.Abs(distanceFromPlayer) < GetComponent<EnemyAttack>().GetAttackRange() - .2f)
        //                 {
        //                     move = new Vector2(0,0);
        //                 }

        //                 if (neverStopFollowing)
        //                 {
        //                     attentionRange = 10000000000;
        //                 }
        //             }
        //             else
        //             {
        //                 followPlayer = false;
        //                 if (sitStillWhenNotFollowing)
        //                 {
        //                     sitStillMultiplier = 0;
        //                 }
        //                 else
        //                 {
        //                     sitStillMultiplier = 1;
        //                 }
        //             }
        //         }

        //         if (followPlayer)
        //         {
        //             rayCastSize.y = 200;
        //             if (distanceFromPlayer > 0 && PlayerBase.Instance.transform.position.x < transform.position.x)
        //             {
        //                 direction = -1;
        //             }
        //             else
        //             {
        //                 direction = 1;
        //             }
        //         }
        //         else
        //         {
        //             rayCastSize.y = rayCastSizeOrig.y;
        //         }

        //         rightWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.right, rayCastSize.x, layerMask);
        //         Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.right * rayCastSize.x, Color.yellow);

        //         if (rightWall.collider != null)
        //         {
        //             if (!followPlayer)
        //             {
        //                 direction = -1;
        //             }
        //             else if (direction == 1)
        //             {
        //                 Jump();
        //             }

        //         }

        //         leftWall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.left, rayCastSize.x, layerMask);
        //         Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + rayCastOffset.y), Vector2.left * rayCastSize.x, Color.blue);

        //         if (leftWall.collider != null)
        //         {
        //             if (!followPlayer)
        //             {
        //                 direction = 1;
        //             }
        //             else if (direction == -1)
        //             {
        //                 Jump();
        //             }
        //         }

        //         rightLedge = Physics2D.Raycast(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down, rayCastSize.y, layerMask);
        //         Debug.DrawRay(new Vector2(transform.position.x + rayCastOffset.x, transform.position.y), Vector2.down * rayCastSize.y, Color.blue);

        //         if (rightLedge.collider == null && direction == 1)
        //         {
        //             direction = -1;
        //         }

        //         leftLedge = Physics2D.Raycast(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down, rayCastSize.y, layerMask);
        //         Debug.DrawRay(new Vector2(transform.position.x - rayCastOffset.x, transform.position.y), Vector2.down * rayCastSize.y, Color.blue);
        //         if (leftLedge.collider == null && direction == -1)
        //         {
        //             direction = 1;
        //         }
        //     // }
        // }
        // enemyBase.animator.SetBool("grounded", grounded);
        // enemyBase.animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        targetVelocity = move * maxSpeed;
    }

    public void Jump()
    {
        if (grounded)
        {
            velocity.y = jumpPower;
            // PlayJumpSound();
            // PlayStepSound();
        }
    }

    // public void PlayStepSound()
    // {
    //     enemyBase.audioSource.pitch = (Random.Range(0.6f, 1f));
    //     enemyBase.audioSource.PlayOneShot(stepSound);
    // }

    // public void PlayJumpSound()
    // {
    //     enemyBase.audioSource.pitch = (Random.Range(0.6f, 1f));
    //     enemyBase.audioSource.PlayOneShot(jumpSound);
    // }
}
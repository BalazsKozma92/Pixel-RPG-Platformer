using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] bool combatEnabled;
    [SerializeField] float inputTimer;
    [SerializeField] float attack1Radius;
    [SerializeField] float attack1Damage;
    [SerializeField] float attack1StunDamage;
    [SerializeField] Transform attack1HitBoxPos;
    [SerializeField] LayerMask damagableLayer;

    bool gotInput;
    bool isAttacking;
    bool isFirstAttack;
    // AttackDetails attackDetails;
    float lastInputTime = Mathf.NegativeInfinity;
    Animator animator;
    PlayerStats playerStats;

    void Awake()
    {
        animator = GetComponent<Animator>();    
        playerStats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        animator.SetBool("canAttack", combatEnabled);    
    }

    void Update()
    {
        CheckForInput();
        CheckAttacks();
    }

    void CheckAttacks()
    {
        if (gotInput)
        {
            if (!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                animator.SetBool("attack1", true);
                animator.SetBool("firstAttack", isFirstAttack);
                animator.SetBool("isAttacking", isAttacking);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            gotInput = false;
        }
    }

    void CheckAttackHitbox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, damagableLayer);

        // attackDetails.damageAmount = attack1Damage;
        // attackDetails.position = transform.position;
        // attackDetails.stunDamageAmount = attack1StunDamage;

        if (detectedObjects.Length == 0)
        {
            AudioPlayer.Instance.PlaySwingSound();
        }
        else
        {
            AudioPlayer.Instance.PlaySwingHitSound();
        }

        foreach (Collider2D collider in detectedObjects)
        {
            // collider.transform.parent.SendMessage("Damage", attackDetails);
        }
    }

    void FinishAttack1()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("attack1", false);
    }

    void CheckForInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && combatEnabled)
        {
            gotInput = true;
            lastInputTime = Time.time;
        }
        // if (value.isPressed && attackTimer <= 0 && !PlayerBase.Instance.frozen)
        // {
        //     attackTimer = timeBetweenAttacks;
        //     attackTimerStarted = true;
        //     animator.SetTrigger("attack");
        // }
    }

    // void Damage(AttackDetails attackDetails)
    // {
    //     if (!Player.Instance.GetDashStatus())
    //     {
    //         int direction = attackDetails.position.x < transform.position.x ? 1 : -1;

    //         playerStats.DecreaseHealth(attackDetails.damageAmount);

    //         Player.Instance.Knockback(direction);
    //     }
    // }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);    
    }

    // [SerializeField] float timeBetweenAttacks = 1.5f;
    // [SerializeField] int baseAttackDamage;
    // [SerializeField] int baseArrowDamage;
    // [SerializeField] int baseMagicDamage;
    // [SerializeField] float attackRange = 1.5f;
    // [SerializeField] float bezierCastRadius = 5f;
    // [SerializeField] ParticleSystem lifeSiphon;
    // [SerializeField] GameObject graphic;
    // [SerializeField] GameObject arrowPool;
    // [SerializeField] float bowForce;
    // [SerializeField] TextMeshProUGUI arrowCountText;

    // int arrowCount;
    // Vector2 lerpPoint;
    // Vector2 aPoint;
    // Vector2 bPoint;
    // float currentAttackDamage;
    // float currentMagicDamage;
    // float attackTimer;
    // bool attackTimerStarted;
    // int targetSide;
    // float chanceToResetCD = 0f;
    // List<string> gemTypesToReset = new List<string>();
    // Arrow currentArrow;

    // public event Action<List<string>> OnProccingCDReset;

    // [SerializeField] float lerpTest = 0;
    // SpriteRenderer dotFirst;
    // bool bezierPressed = false;
    // float coroutineElapsedTime = 2f;
    // Dictionary<Vector2[], GameObject> curveDict = new Dictionary<Vector2[], GameObject>();

    // Animator animator;

    // private void Awake()
    // {
    //     animator = GetComponentInChildren<Animator>();
    // }

    // private void Start()
    // {
    //     arrowCount = arrowPool.transform.childCount;
    //     arrowCountText.text = arrowCount.ToString();
    //     attackTimer = 0;
    //     currentAttackDamage = baseAttackDamage;
    //     currentMagicDamage = baseMagicDamage;
    // }

    // public void SetAttackDamage(float damage)
    // {
    //     currentAttackDamage += damage;
    //     GameManager.Instance.SetAttackDamageText(currentAttackDamage);
    // }

    // public void SetMagicDamage(int damage)
    // {
    //     currentMagicDamage += damage;
    // }

    // public float GetAttackDamage()
    // {
    //     return currentAttackDamage;
    // }

    // public float GetMagicDamage()
    // {
    //     return currentMagicDamage;
    // }

    // public void ShootingArrow(float attackModifier, float attackRangeModifier, bool isMagicDamage)
    // {
    //     float thisAttackDamage = baseArrowDamage * attackModifier;
    //     AudioPlayer.Instance.PlayBowSound();
    //     Vector2 forceToAdd = graphic.transform.localScale.x > 0 ? Vector2.right * bowForce: Vector2.left * bowForce;
    //     currentArrow.gameObject.SetActive(true);
    //     currentArrow.transform.position = new Vector2(transform.position.x, transform.position.y - .3f);
    //     currentArrow.transform.localScale = new Vector2(Math.Sign(graphic.transform.localScale.x) * currentArrow.transform.localScale.x, currentArrow.transform.localScale.y);
    //     currentArrow.attackDamage = thisAttackDamage;
    //     currentArrow.SetAlreadyDisabled(false);
    //     currentArrow.AddingVelocityToFly(forceToAdd);
    //     arrowCount -= 1;
    //     arrowCountText.text = arrowCount.ToString();
    // }

    // public void IncreaseArrowCount(int count)
    // {
    //     arrowCount += count;
    //     arrowCountText.text = arrowCount.ToString();
    // }

    // void Update()
    // {   
        // if (Mouse.current.leftButton.wasPressedThisFrame && attackTimer <= 0 && !PlayerBase.Instance.frozen)
        // {
        //     attackTimer = timeBetweenAttacks;
        //     attackTimerStarted = true;
        //     animator.SetTrigger("attack");
        // }
        // else if (Mouse.current.rightButton.wasPressedThisFrame && attackTimer <= 0 && !PlayerBase.Instance.frozen)
        // {
        //     attackTimer = timeBetweenAttacks;
        //     attackTimerStarted = true;
        //     for (int i = 0; i < arrowPool.transform.childCount; i++)
        //     {
        //         currentArrow = arrowPool.transform.GetChild(i).GetComponent<Arrow>();
        //         if (!currentArrow.gameObject.activeSelf)
        //         {
        //             animator.SetTrigger("usebow");
        //             break;
        //         }
        //     }
        // }

        // if (attackTimerStarted)
        // {
        //     attackTimer -= Time.deltaTime;
        //     if (attackTimer <= 0)
        //     {
        //         attackTimerStarted = false;
        //     }
        // }

        // BezierTest();

        // if (bezierPressed)
        // {
        //     foreach(KeyValuePair<Vector2[],GameObject> curve in curveDict)
        //     {
        //         if (curve.Value)
        //         {
        //             curve.Key[0].x = curve.Value.transform.position.x;
        //             curve.Key[0].y = curve.Value.transform.position.y + .5f;

        //             curve.Key[2].x = (transform.position + (curve.Value.transform.GetChild(0).position - transform.position).normalized * 1.2f).x;
        //             curve.Key[2].y = (transform.position + (curve.Value.transform.GetChild(0).position - transform.position).normalized * 1.2f).y;

        //             curve.Key[3].x = transform.position.x;
        //             curve.Key[3].y = transform.position.y + .5f;
                
        //             curve.Key[1].x = (curve.Key[0].x + curve.Key[2].x) / 2;
        //             curve.Key[1].y = curve.Key[2].y;

        //             if (curve.Key[2].y < curve.Key[0].y)
        //             {
        //                 curve.Key[2].y += .5f + (curve.Key[0].y - curve.Key[2].y);
        //                 curve.Key[1].y = curve.Key[2].y;
        //             }

        //             if (coroutineElapsedTime >= 2f)
        //             {
        //                 coroutineElapsedTime = 0;
        //                 StartCoroutine(StartLerping());
        //             }
        //         }
        //     }
        // }
    // }

    // IEnumerator StartLerping() {
    //     GameObject lifeSiphonInstance = Instantiate(lifeSiphon.gameObject, new Vector2(transform.position.x, transform.position.y), Quaternion.identity, null) as GameObject;

    //     while (coroutineElapsedTime < 2f)
    //     {
    //         foreach(KeyValuePair<Vector2[],GameObject> curve in curveDict)
    //         {
    //             Vector2 a = Vector2.Lerp(curve.Key[0], curve.Key[2], coroutineElapsedTime);
    //             Vector2 b = Vector2.Lerp(curve.Key[1], curve.Key[2], coroutineElapsedTime);
    //             Vector2 c = Vector2.Lerp(curve.Key[2], curve.Key[3], coroutineElapsedTime);

    //             aPoint = Vector2.Lerp(a, b, coroutineElapsedTime);
    //             bPoint = Vector2.Lerp(b, c, coroutineElapsedTime);
    //             lerpPoint = Vector2.Lerp(aPoint, bPoint, coroutineElapsedTime);
    //             if (lifeSiphonInstance)
    //             {
    //                 lifeSiphonInstance.transform.position = lerpPoint;
    //             }
    //             coroutineElapsedTime += Time.deltaTime;
    //             yield return new WaitForSecondsRealtime(Time.deltaTime);
    //         }
    //     }
    // }

    // public void SetChanceToResetCD(float chanceToAdd, string gemType)
    // {
    //     chanceToResetCD += chanceToAdd;
    //     if (chanceToAdd > 0)
    //     {
    //         if (!gemTypesToReset.Contains(gemType))
    //         {
    //             gemTypesToReset.Add(gemType);
    //         }
    //     } 
    //     else 
    //     {
    //         if (gemTypesToReset.Contains(gemType))
    //         {
    //             gemTypesToReset.Remove(gemType);
    //         }
    //     }
    // }

    // void BezierTest()
    // {
    //     if (Keyboard.current.bKey.wasPressedThisFrame && !bezierPressed)
    //     {
    //         bezierPressed = true;
    //         RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, bezierCastRadius, Vector2.up, bezierCastRadius, LayerMask.GetMask("Enemies"));

    //         if (hit == null) {return;}

    //         foreach (var enemy in hit)
    //         {
    //             Vector2 firstVector = new Vector2(enemy.transform.position.x, enemy.transform.position.y + .5f);
    //             Vector2 thirdVector = transform.position + (enemy.transform.GetChild(0).position - transform.position).normalized * 1.2f;
    //             Vector2 fourthVector = new Vector2(transform.position.x, transform.position.y);
    //             Vector2 secondVector = new Vector2((firstVector.x + thirdVector.x) / 2, thirdVector.y);

    //             if (thirdVector.y < firstVector.y) {
    //                 thirdVector.y += .5f + (firstVector.y - thirdVector.y);
    //                 secondVector.y = thirdVector.y;
    //             }

    //             Vector2[] bezierPoints = new Vector2[4];
    //             bezierPoints[0] = firstVector;
    //             bezierPoints[1] = secondVector;
    //             bezierPoints[2] = thirdVector;
    //             bezierPoints[3] = fourthVector;

    //             curveDict.Add(bezierPoints, enemy.transform.gameObject);
    //         }
    //     }
    // }

    // void OnDrawGizmosSelected()
    // {
        // Gizmos.color = Color.yellow;
        // foreach(KeyValuePair<Vector2[],GameObject> curve in curveDict)
        // {
        //     if (curve.Value) {
        //         foreach (var point in curve.Key)
        //         {
        //             Gizmos.DrawWireSphere(point, .25f);
        //         }
        //     }
        // }
        // Gizmos.color = Color.red;
        // Gizmos.DrawWireSphere(aPoint, .2f);
        // Gizmos.DrawWireSphere(bPoint, .2f);
        // Gizmos.color = Color.green;
        // Gizmos.DrawWireSphere(lerpPoint, .25f);
    // }

    // public void CheckForEnemies(float attackModifier, float attackRangeModifier, bool isMagicDamage)
    // {
    //     float thisAttackDamage;
    //     float thisAttackRange = attackRangeModifier * attackRange;
    //     if (isMagicDamage)
    //     {
    //         thisAttackDamage = currentMagicDamage * attackModifier;
    //     }
    //     else
    //     {
    //         thisAttackDamage = currentAttackDamage * attackModifier;
    //     }
    //     Vector2 direction = graphic.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    //     RaycastHit2D[] hit = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y - .2f), direction, thisAttackRange, LayerMask.GetMask("Enemies"));

    //     Debug.DrawLine(new Vector2(transform.position.x, transform.position.y - .2f), new Vector2(transform.position.x, transform.position.y - .2f) + direction * thisAttackRange, Color.red, 1f);

    //     if (hit.Length == 0)
    //     {   
    //         AudioPlayer.Instance.PlaySwingSound();
    //         return;
    //     }
    //     float thisChance = UnityEngine.Random.Range(0f, 1f);
    //     if (thisChance < chanceToResetCD)
    //     {
    //         OnProccingCDReset(gemTypesToReset);
    //     }
    //     AudioPlayer.Instance.PlaySwingHitSound();

    //     foreach (var enemy in hit)
    //     {
    //         targetSide = transform.position.x < enemy.transform.position.x ? 1 : -1;
    //         enemy.transform.GetComponentInChildren<EnemyBase>().GetHurt(targetSide, thisAttackDamage);
    //     }

    // }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    [Header("Health loot")]
    [SerializeField] GameObject healthOrbLoot;
    [Range(0f, 1f)]
    [SerializeField] float healthOrbChance;
    [SerializeField] float healthOrbMultiplier;

    [Header("Loot and chances")]
    [SerializeField] GameObject[] lootTable;
    [Range(0f, 1f)]
    [SerializeField] float[] chances;

    [SerializeField] float[] multiplier;

    [Header("Item to drop 100%")]
    [SerializeField] GameObject certainLoot;

    int loopCount;
    EnemyBase enemyBase;

    private void Awake()
    {
        loopCount = lootTable.Length;
        enemyBase = GetComponentInParent<EnemyBase>();
    }

    private void Start() {
        // enemyBase.OnEnemyDeath += DropGoodies;
        GemBuffManager.Instance.HealthOrbDropIncreased += SetHealthOrbChance;
    }

    public void SetHealthOrbChance(float plusPercentage)
    {
        healthOrbChance += plusPercentage;
    }

    void DropGoodies()
    {
        transform.SetParent(null);
        for (int i = 0; i < lootTable.Length; i++)
        {
            if (Random.Range(0f, 1f) < chances[i])
            {
                float randomSpread = Random.Range(0f, 2f);
                int randomNumber = (int)Mathf.Ceil(randomSpread * multiplier[i]);
                StartCoroutine(DropRandoms(lootTable[i], randomNumber));   
            }
        }

        if (Random.Range(0f, 1f) < healthOrbChance)
        {
            float randomSpread = Random.Range(0f, 2f);
            int randomNumber = (int)Mathf.Ceil(randomSpread * healthOrbMultiplier);
            StartCoroutine(DropRandoms(healthOrbLoot, randomNumber));   
        }

        if (certainLoot)
        {
            DropCertain();
        }

        Invoke("DestroyMyself", 2f);
    }

    void DestroyMyself()
    {
        Destroy(gameObject);
    }

    void DropCertain()
    {
        Instantiate(certainLoot, transform.position, Quaternion.identity);
    }

    IEnumerator DropRandoms(GameObject loot, int randomNumber)
    {
        while (randomNumber != 0)
        {
            for (int j = 0; j < randomNumber; j++)
            {
                GameObject instantiatedLoot = Instantiate(loot, new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(1.5f, 2f)), Quaternion.identity);
                instantiatedLoot.transform.SetParent(null);
                randomNumber -= 1;
                yield return new WaitForSeconds(.02f);
            }
        }
    }
}

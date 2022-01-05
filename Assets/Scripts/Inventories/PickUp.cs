using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health = 1;

    [Header("Gold")]
    [SerializeField] int certainGoldAmount;
    [SerializeField] bool isRandomGoldAmount;
    [SerializeField] int minGoldAmount = 1;
    [SerializeField] int maxGoldAmount;
    
    [Header("Vigour gem fragment")]
    [SerializeField] int certainVigourFragmentCount;
    [SerializeField] bool isRandomVigourAmount;
    [SerializeField] int minVigourAmount = 1;
    [SerializeField] int maxVigourAmount;

    [Header("Death gem fragment")]
    [SerializeField] int certainDeathFragmentCount;
    [SerializeField] bool isRandomDeathAmount;
    [SerializeField] int minDeathAmount = 1;
    [SerializeField] int maxDeathAmount;

    [Header("Balance gem fragment")]
    [SerializeField] int certainBalanceFragmentCount;
    [SerializeField] bool isRandomBalanceAmount;
    [SerializeField] int minBalanceAmount = 1;
    [SerializeField] int maxBalanceAmount;

    [Header("Quest item")]
    [SerializeField] QuestItem questItem;

    private void Start() {
        if (isRandomGoldAmount)
        {
            certainGoldAmount = Random.Range(minGoldAmount, maxGoldAmount);
        }
        if (isRandomVigourAmount)
        {
            certainVigourFragmentCount = Random.Range(minVigourAmount, maxVigourAmount);
        }
        if (isRandomDeathAmount)
        {
            certainDeathFragmentCount = Random.Range(minDeathAmount, maxDeathAmount);
        }
        if (isRandomBalanceAmount)
        {
            certainBalanceFragmentCount = Random.Range(minBalanceAmount, maxBalanceAmount);
        }
        if ((certainGoldAmount + certainVigourFragmentCount + certainDeathFragmentCount + certainBalanceFragmentCount + health) == 0 && questItem == QuestItem.None)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            string myTag = transform.tag;
            switch (myTag)
            {
                case "gold":
                    GameManager.Instance.ChangeGoldBy(certainGoldAmount);
                    break;
                case "vigourFragment":
                    GameManager.Instance.ChangeVigourFragmentAmount(certainVigourFragmentCount);
                    break;
                case "deathFragment":
                    GameManager.Instance.ChangeDeathFragmentAmount(certainDeathFragmentCount);
                    break;
                case "balanceFragment":
                    GameManager.Instance.ChangeBalanceFragmentAmount(certainBalanceFragmentCount);
                    break;
                case "healthPickup":
                    other.GetComponentInChildren<Combat>().SetHealth(health);
                    break;
            }
            if (questItem != QuestItem.None)
            {
                QuestItems.Instance.addQuestItem(questItem);
            }

            Destroy(gameObject);
        }
    }
}

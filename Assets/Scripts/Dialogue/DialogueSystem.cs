using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public enum PaymentTypes
{
    None,
    Gold,
    VigourFragment,
    DeathFragment,
    BalanceFragment
}

[RequireComponent(typeof(ActivateDialogueScript))]
public class DialogueSystem : MonoBehaviour
{
    [SerializeField] private string characterName;

    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI textAcceptedDoneText;

    [TextArea(1, 10)]
    [SerializeField] private string[] firstSetOfMessages;
    [TextArea(1, 10)]
    [SerializeField] private string[] choiceOptions;
    [TextArea(1, 10)]
    [SerializeField] private string[] choiceOptionsDoneAlready;
    [TextArea(1, 10)]
    [SerializeField] private string[] monologueIfQuestNotDone;
    [TextArea(1, 10)]
    [SerializeField] private string[] monologueIfQuestOrPurchaseDone;
    [TextArea(1, 10)]
    [SerializeField] private string alreadyDoneMessage;

    [SerializeField] private bool isMerchant;
    [SerializeField] private bool generalNPC;
    [SerializeField] private bool isGivingAQuest = false;

    [SerializeField] string questName;
    [SerializeField] private QuestItem questItemToGive;
    [Header("Requirements")]
    [SerializeField] private int requiredKillAmount;
    [SerializeField] private EnemyName enemyTypeToKill;
    [SerializeField] private int requiredAmount;
    [SerializeField] private PaymentTypes requiredType;
    [SerializeField] private QuestItem requiredQuestItem;

    [Header("Rewards")]
    [SerializeField] private int rewardAmount;
    [SerializeField] private PaymentTypes rewardType;

    [SerializeField] private GameObject[] activateOnCompletion;
    [SerializeField] private GameObject[] deActivateOnCompletion;

    [SerializeField] private float typeSpeed = 1f;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    [SerializeField] private TextMeshProUGUI firstChoice;
    [SerializeField] private TextMeshProUGUI secondChoice;
    [SerializeField] private Animator animator;

    QuestItems questItems;
    private bool alreadyBought = false;
    private bool rewarded = false;
    private bool isQuestGiven = false;
    private bool isQuestDone = false;
    private bool typing = false;
    private bool horizontalKeyIsDown = false;
    private bool submitKeyIsDown = false; 
    private bool closeNow = false;
    private bool playerIsHere = false;
    private bool dialogueCanvasActive = false;
    private bool alreadyTookRequiredItem = false;
    private int currentIndexFirstSet = 0;
    private int currentIndexBeforeCompletion = 0;
    private int currentIndexAfterCompletion = 0;
    private Vector2 moveInput;

    private void Awake() {
        questItems = FindObjectOfType<QuestItems>();
    }

    private void Update() {

        if (playerIsHere && Keyboard.current.eKey.wasPressedThisFrame && !typing)
        {
            if (!dialogueCanvasActive)
            {
                dialogueCanvasActive = true;
                AudioPlayer.Instance.PlayUIOpen();
            }
            npcName.text = characterName;
            // PlayerBase.Instance.Freeze(true);
            HandleInput();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame && !submitKeyIsDown && animator.GetBool("hasChoices") && !typing)
        {
            ChooseAnOption();
        }

        if (submitKeyIsDown)
        {
            if (!typing)
            {
                submitKeyIsDown = false;
            }
        }

        TriggerAnimatorChoiceSelection();
    }

    private void HandleInput()
    {
        if (closeNow)
        {
            StartCoroutine(Close());
            return;
        }

        dialogueCanvas.GetComponent<Canvas>().enabled = true;
        if (!typing || !animator.GetBool("hasChoices"))
        {
            if (generalNPC)
            {
                ShowFirstMessages();
            }
            else
            {
                if ((isMerchant && !alreadyBought) || (!isQuestGiven && !isQuestDone))
                {
                    ShowFirstMessages();
                }
                else if (isQuestGiven && !isQuestDone)
                {
                    if (CheckIfCompleted())
                    {
                        isQuestDone = true;
                        if (isGivingAQuest)
                        {
                            QuestsUI.Instance.QuestCompleted(questName);
                            textAcceptedDoneText.text = "Quest completed: " + questName;
                            textAcceptedDoneText.GetComponent<Animator>().SetTrigger("Appear");
                            GiveReward();
                            AudioPlayer.Instance.PlayQuestDone();
                        }
                        ShowAfterCompletionMessages();
                        closeNow = true;
                    }
                    else
                    {
                        ShowBeforeCompletionMessages();
                        closeNow = true;
                    }
                }
                else if ((isMerchant && alreadyBought) || isQuestDone)
                {
                    ShowAfterCompletionMessages();
                    closeNow = true;
                }
            }
        }
    }

    private void TriggerAnimatorChoiceSelection()
    {
        if (moveInput.x != 0 && animator.GetBool("hasChoices") && !horizontalKeyIsDown)
        {
            if (animator.GetInteger("choiceSelection") == 1)
            {
                animator.SetInteger("choiceSelection", 2);
            }
            else
            {
                animator.SetInteger("choiceSelection", 1);
            }
           horizontalKeyIsDown = true;
        }

        if (horizontalKeyIsDown && moveInput.x == 0)
        {
            horizontalKeyIsDown = false;
        }
    }

    void ResetFirstOption()
    {
        firstChoice.transform.parent.gameObject.SetActive(true);
    }

    private void ChooseAnOption()
    {
        submitKeyIsDown = true;
        ShowChoices(false);
        animator.SetBool("hasChoices", false);
        if (!isGivingAQuest)
        {
            animator.SetInteger("choiceSelection", 2);
            Invoke("ResetFirstOption", 3f);
        }
        if (animator.GetInteger("choiceSelection") == 1)
        {
            StartCoroutine(Close());
            return;
        }
        else
        {
            if (!isMerchant)
            {
                isQuestGiven = true;
                if (questItemToGive != QuestItem.None)
                {
                    questItems.addQuestItem(questItemToGive);
                }
                if (isGivingAQuest)
                {
                    AudioPlayer.Instance.PlayQuestAccepted();
                    QuestsUI.Instance.AddQuest(questName, requiredType, requiredQuestItem, requiredAmount, enemyTypeToKill, requiredKillAmount, rewardType, rewardAmount);
                }
                CheckIfCompletedAlready();
            }
            else 
            {
                CheckIfCompleted();
                StartCoroutine(Close());
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerIsHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }

    private void OnMove(InputValue value) {
        moveInput = value.Get<Vector2>();
    }

    private void ShowFirstMessages()
    {
        if (currentIndexFirstSet < firstSetOfMessages.Length)
        {
            StartCoroutine(Advance(firstSetOfMessages, currentIndexFirstSet));
        }
    }

    private void CheckIfCompletedAlready()
    {
        typing = true;
        if (CheckIfCompleted())
        {
            if (isGivingAQuest)
            {
                QuestsUI.Instance.QuestCompleted(questName);
                textAcceptedDoneText.text = "Quest completed: " + questName;
                textAcceptedDoneText.GetComponent<Animator>().SetTrigger("Appear");
                GiveReward();
                AudioPlayer.Instance.PlayQuestDone();
            }
            npcDialogueText.text = "";
            StartCoroutine(TypeText(alreadyDoneMessage));
            closeNow = true;
            isQuestDone = true;
        }
        else
        {
            textAcceptedDoneText.text = "Quest accepted: " + questName;
            textAcceptedDoneText.GetComponent<Animator>().SetTrigger("Appear");
            typing = false;
            StartCoroutine(Close());
        }
    }

    private bool CheckIfCompleted()
    {
        if (requiredType == PaymentTypes.None && enemyTypeToKill == EnemyName.None && requiredQuestItem == QuestItem.None)
        {
            return true;
        }

        switch (requiredType)
        {
            case PaymentTypes.Gold:
                if (alreadyTookRequiredItem) {
                    return true;
                }
                if (requiredAmount <= GameManager.Instance.GetGold())
                {
                    if (!alreadyTookRequiredItem)
                    {
                        alreadyTookRequiredItem = true;
                        GameManager.Instance.ChangeGoldBy(-requiredAmount);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            case PaymentTypes.VigourFragment:
                if (alreadyTookRequiredItem) {
                    return true;
                }
                if (requiredAmount <= GameManager.Instance.GetVigourFragments())
                {
                    if (!alreadyTookRequiredItem)
                    {
                        alreadyTookRequiredItem = true;
                        GameManager.Instance.ChangeVigourFragmentAmount(-requiredAmount);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            case PaymentTypes.DeathFragment:
                if (alreadyTookRequiredItem) {
                    return true;
                }
                if (requiredAmount <= GameManager.Instance.GetDeathFragments())
                {
                    if (!alreadyTookRequiredItem)
                    {
                        alreadyTookRequiredItem = true;
                        GameManager.Instance.ChangeDeathFragmentAmount(-requiredAmount);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            case PaymentTypes.BalanceFragment:
                if (alreadyTookRequiredItem) {
                    return true;
                }
                if (requiredAmount <= GameManager.Instance.GetBalanceFragments())
                {
                    if (!alreadyTookRequiredItem)
                    {
                        alreadyTookRequiredItem = true;
                        GameManager.Instance.ChangeBalanceFragmentAmount(-requiredAmount);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
        }

        switch (enemyTypeToKill)
        {
            case EnemyName.Slime:
                if (requiredKillAmount <= GameManager.Instance.GetKillCount(EnemyName.Slime))
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        if (requiredQuestItem != QuestItem.None)
        {
            if (alreadyTookRequiredItem) {
                return true;
            }
            if (questItems.IsQuestItemInInventory(requiredQuestItem) && !alreadyTookRequiredItem)
            {
                alreadyTookRequiredItem = true;
                questItems.removeQuestItem(requiredQuestItem);
                return true;
            }
            else
            {
                return false;   
            }
                
        }

        return false;
    }

    void ActivateObjects()
    {
        for (int i = 0; i < activateOnCompletion.Length; i++)
        {
            activateOnCompletion[i].SetActive(true);
        }
    }

    void DeActivateObjects()
    {
        for (int i = 0; i < deActivateOnCompletion.Length; i++)
        {
            deActivateOnCompletion[i].SetActive(false);
        }
    }

    private void GiveReward()
    {
        if (!rewarded)
        {
            rewarded = true;
            Invoke("ActivateObjects", .5f);
            Invoke("DeActivateObjects", .5f);

            switch (rewardType)
            {
                case PaymentTypes.Gold:
                    GameManager.Instance.ChangeGoldBy(rewardAmount);
                    AudioPlayer.Instance.PlayReceiveGold();
                    break;
                case PaymentTypes.VigourFragment:
                    GameManager.Instance.ChangeVigourFragmentAmount(rewardAmount);
                    break;
                case PaymentTypes.DeathFragment:
                    GameManager.Instance.ChangeDeathFragmentAmount(rewardAmount);
                    break;
                case PaymentTypes.BalanceFragment:
                    GameManager.Instance.ChangeBalanceFragmentAmount(rewardAmount);
                    break;
                default:
                    return;
            }
        }
    }

    private void ShowBeforeCompletionMessages()
    {
        if (currentIndexBeforeCompletion < monologueIfQuestNotDone.Length)
        {
            StartCoroutine(Advance(monologueIfQuestNotDone, currentIndexBeforeCompletion));
        }
    }

    private void ShowAfterCompletionMessages()
    {
        if (currentIndexAfterCompletion < monologueIfQuestOrPurchaseDone.Length)
        {
            StartCoroutine(Advance(monologueIfQuestOrPurchaseDone, currentIndexAfterCompletion));
        }
    }

    private IEnumerator Advance(string[] messages, int currentIndex)
    {
        AudioPlayer.Instance.PlayNextDialogue();
        typing = true;
        npcDialogueText.text = "";
        StartCoroutine(TypeText(messages[currentIndex]));

        yield return new WaitForSeconds(.4f);

        if (generalNPC)
        {
            currentIndexFirstSet = Random.Range(0, firstSetOfMessages.Length);
            closeNow = true;
        }
        else
        {
            if ((isMerchant && !alreadyBought) || (!isQuestGiven && !isQuestDone))
            {
                if (currentIndex == firstSetOfMessages.Length - 1)
                {
                    ShowChoices(true);
                }
                currentIndexFirstSet++;
            }
            if (isQuestGiven && !isQuestDone)
            {
                currentIndexBeforeCompletion = Random.Range(0, monologueIfQuestNotDone.Length);
            }
            if ((isMerchant && alreadyBought) || isQuestDone)
            {
                currentIndexAfterCompletion = Random.Range(0, monologueIfQuestOrPurchaseDone.Length);
            }
        }
    }

    private IEnumerator TypeText(string message)
    {
        int cPos = 0;
        foreach (char c in message)
        {
            cPos++;
            if (cPos != 0 && cPos == message.Length)
            {
                typing = false;
            }

            npcDialogueText.text += c;
            // audioSource.PlayOneShot(typeSounds[Random.Range(0, typeSounds.Length)], Random.Range(.3f, .5f));
            yield return new WaitForSeconds(typeSpeed / 100);
        }
    }

    private IEnumerator Close()
    {
        dialogueCanvasActive = false;
        AudioPlayer.Instance.PlayUIClose();
        if (!generalNPC)
        {
            currentIndexFirstSet = 0;
        }
        npcDialogueText.text = "";
        playerIsHere = false;
        dialogueCanvas.GetComponent<Canvas>().enabled = false;
        closeNow = false;
        yield return new WaitForSeconds(.2f);
        // PlayerBase.Instance.Freeze(false);
    }

    private void ShowChoices(bool show)
    {
        animator.SetBool("hasChoices", show);
        firstChoice.enabled = show;
        secondChoice.enabled = show;
        if (!isGivingAQuest)
        {
            firstChoice.transform.parent.gameObject.SetActive(false);
        }

        if (CheckIfCompleted())
        {
            // isQuestDone = false;
            if (choiceOptionsDoneAlready != null)
            {
                firstChoice.GetComponent<TextMeshProUGUI>().text = choiceOptionsDoneAlready[0];
                secondChoice.GetComponent<TextMeshProUGUI>().text = choiceOptionsDoneAlready[1];    
            }
        }
        else
        {
            firstChoice.GetComponent<TextMeshProUGUI>().text = choiceOptions[0];
            secondChoice.GetComponent<TextMeshProUGUI>().text = choiceOptions[1];
        }
    }
}

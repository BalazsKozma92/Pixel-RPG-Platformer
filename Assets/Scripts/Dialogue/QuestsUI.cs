using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] questNameHolders;
    [SerializeField] TextMeshProUGUI questDescriptionText;

    Dictionary<string, string> questDict = new Dictionary<string, string>();

    static QuestsUI instance;
    public static QuestsUI Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<QuestsUI>();
            return instance;
        }
    }

    private void Start() {
        InitializeDictionary();
        questDescriptionText.text = "";
    }

    void InitializeDictionary()
    {
        questDict.Add("Slime problem", "The blacksmith asked me to kill the slimes east of Ardwood. Those disgusting creatures destroyed almost half of the crops already. I have to stop them before they devour everything. My father never would've let them come even near of the village... I miss him.");
        questDict.Add("Into the ruins", "The innkeeper told me that the key (hopefully) opens the gate of the nearby castle. Well, it's not really a castle anymore, only ruins. It's been standing there abandoned for the last 2 decades. I'm afraid to go there, but honestly, I'm more excited!");
        questDict.Add("Placeholder quest2", "Placeholder description.");
    }

    public void EnableThisQuestText(int index)
    {
        AudioPlayer.Instance.PlayQuestLogChange();
        questDescriptionText.text = questDict[questNameHolders[index].text];
        for (int i = 0; i < questNameHolders.Length; i++)
        {
            questNameHolders[i].transform.parent.GetComponent<Toggle>().SetIsOnWithoutNotify(false);
        }
        questNameHolders[index].transform.parent.GetComponent<Toggle>().SetIsOnWithoutNotify(true);
    }

    public void SetDescriptionTextToEmpty()
    {
        questDescriptionText.text = "";
    }

    public void AddToQuestText(string questName, string text)
    {
        questDict[questName] += "\n\n" + text + "\n\n";
    }

    public void AddQuest(string questName, PaymentTypes requiredType, QuestItem questItem, int requiredAmount, EnemyName enemyTypeToKill, int requiredKillAmount, PaymentTypes rewardType, int rewardAmount)
    {
        string requiredTypeString;
        string requiredKillString;
        string rewardTypeString;

        for (int i = 0; i < questNameHolders.Length; i++)
        {
            if (questNameHolders[i].text == "")
            {
                questNameHolders[i].text = questName;
                questNameHolders[i].transform.parent.GetComponent<Toggle>().interactable = true;
                break;
            }
        }
        if (requiredType != PaymentTypes.None)
        {
            switch (requiredType)
            {
                case PaymentTypes.Gold:
                    requiredTypeString = "\n <color=yellow>Collect: " + requiredAmount.ToString() + " gold";
                    break;
                case PaymentTypes.VigourFragment:
                    requiredTypeString = "\n <color=#B3A91C>Collect: " + requiredAmount.ToString() + " vigour fragment";
                    break;
                case PaymentTypes.DeathFragment:
                    requiredTypeString = "\n <color=#D52EF6>Collect: " + requiredAmount.ToString() + " death fragment";
                    break;
                case PaymentTypes.BalanceFragment:
                    requiredTypeString = "\n <color=lightblue>Collect: " + requiredAmount.ToString() + " balance fragment";
                    break;
                default:
                    requiredTypeString = "";
                    break;
            }
            questDict[questName] = questDict[questName] + requiredTypeString;
            if (requiredAmount > 1)
            {
                questDict[questName] = questDict[questName] + "s";
            }
            questDict[questName] = questDict[questName] + "</color>";
        }

        if (enemyTypeToKill != EnemyName.None)
        {
            switch (enemyTypeToKill)
            {
                case EnemyName.Slime:
                    requiredKillString = " slime";
                    break;
                case EnemyName.Skeleton:
                    requiredKillString = " skeleton";
                    break;
                case EnemyName.Trap:
                    requiredKillString = " trap";
                    break;
                default:
                    requiredKillString = "";
                    break;
            }
            questDict[questName] = questDict[questName] + "\n <color=red>Kill: " + requiredKillAmount.ToString() + requiredKillString;
            if (requiredKillAmount > 1)
            {
                questDict[questName] = questDict[questName] + "s";
            }
            questDict[questName] = questDict[questName] + "</color>";
        }

        if (rewardType != PaymentTypes.None)
        {
            switch (rewardType)
            {
                case PaymentTypes.Gold:
                    rewardTypeString = "\n <color=yellow>Reward: " + rewardAmount.ToString() + " gold";
                    break;
                case PaymentTypes.VigourFragment:
                    rewardTypeString = "\n <color=#B3A91C>Reward: " + rewardAmount.ToString() + " vigour fragment";
                    break;
                case PaymentTypes.DeathFragment:
                    rewardTypeString = "\n <color=#D52EF6>Reward: " + rewardAmount.ToString() + " death fragment";
                    break;
                case PaymentTypes.BalanceFragment:
                    rewardTypeString = "\n <color=lightblue>Reward: " + rewardAmount.ToString() + " balance fragment";
                    break;
                default:
                    rewardTypeString = "";
                    break;
            }
            questDict[questName] = questDict[questName] + rewardTypeString;
            if (rewardAmount > 1)
            {
                questDict[questName] = questDict[questName] + "s";
            }
            questDict[questName] = questDict[questName] + "</color>";
        }
    }

    public void QuestCompleted(string questName)
    {
        for (int i = 0; i < questNameHolders.Length; i++)
        {
            if (questNameHolders[i].text == questName)
            {
                Toggle toggle = questNameHolders[i].transform.parent.GetComponent<Toggle>();
                ColorBlock colorBlock = toggle.colors;
                colorBlock.normalColor = new Color(.22f, .4f, .16f, .5f);
                colorBlock.highlightedColor = new Color(.22f, .4f, .16f, 1f);
                colorBlock.pressedColor = new Color(.22f, .4f, .16f, 1f);
                colorBlock.selectedColor = new Color(.22f, .4f, .16f, 1f);
                toggle.colors = colorBlock;
            }
        }
        questDict[questName] = questDict[questName] + "\n\n <color=green>COMPLETED</color>";
        questDescriptionText.text = questDict[questName];
    }
}

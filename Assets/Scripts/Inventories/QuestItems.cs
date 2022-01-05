using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestItem
{ 
    None,
    Village1CastleKeyDropped,
    Village1CastleKey,
    Village1CastleStrangeObject
}

public class QuestItems : MonoBehaviour
{
    [SerializeField] GameObject questList;

    [Header("Quest items")]
    [SerializeField] GameObject village1CastleKey;
    [SerializeField] GameObject village1CastleKeyDropped;
    [SerializeField] GameObject village1CastleStrangeObject;

    Dictionary<QuestItem, GameObject> questItemDict = new Dictionary<QuestItem, GameObject>();
    Dictionary<QuestItem, GameObject> currentQuestItems = new Dictionary<QuestItem, GameObject>();
    List<QuestItem> questItemHolder = new List<QuestItem>();

    static QuestItems instance;
    public static QuestItems Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<QuestItems>();
            return instance;
        }
    }

    private void Start() {
        questItemDict.Add(QuestItem.Village1CastleKey, village1CastleKey);
        questItemDict.Add(QuestItem.Village1CastleKeyDropped, village1CastleKeyDropped);
        questItemDict.Add(QuestItem.Village1CastleStrangeObject, village1CastleStrangeObject);
    }

    public void addQuestItem(QuestItem item)
    {
        questItemHolder.Add(item);
        GameObject instantiatedQuestItem = Instantiate(questItemDict[item], questList.transform.position, Quaternion.identity);
        instantiatedQuestItem.transform.SetParent(questList.transform);
        instantiatedQuestItem.transform.localScale = new Vector3(1,1,1);
        currentQuestItems.Add(item, instantiatedQuestItem);
    }

    public void removeQuestItem(QuestItem item)
    {
        questItemHolder.Remove(item);
        GameObject questItemToDestroy = currentQuestItems[item];
        currentQuestItems.Remove(item);
        Destroy(questItemToDestroy);
    }

    public bool IsQuestItemInInventory(QuestItem item)
    {
        return questItemHolder.Contains(item);
    }
}

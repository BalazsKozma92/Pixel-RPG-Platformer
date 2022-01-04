using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ToolTip tooltip;
    RectTransform rectTransform;
    public string header;
    public string content;
    public string helmetText;
    public string torsoText;
    public string swordText;
    public string feetText;
    public bool isAbility;

    private void Awake() {
    }

    private void Start() {
        tooltip = FindObjectOfType<ToolTipSystem>().transform.GetChild(0).GetComponent<ToolTip>();
        rectTransform = tooltip.GetComponent<RectTransform>();
    }

    public void SetText(string header, string content, string helmet, string torso, string sword, string feet) {
        this.header = header;
        this.content = content;
        this.helmetText = helmet;
        this.torsoText = torso;
        this.swordText = sword;
        this.feetText = feet;
    }

    public void SetText(string header, string content, string helmet, string torso, string sword, string feet, bool isAbility) {
        this.header = header;
        this.content = content;
        this.helmetText = helmet;
        this.torsoText = torso;
        this.swordText = sword;
        this.feetText = feet;
        this.isAbility = isAbility;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (string.IsNullOrEmpty(header) && string.IsNullOrEmpty(content)) {
            return;
        }
        float pivotX = eventData.position.x / Screen.width;
        float pivotY = eventData.position.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);

        tooltip.transform.position = eventData.position;
        if (string.IsNullOrEmpty(helmetText))
        {
            ToolTipSystem.Show(content, header, helmetText, torsoText, swordText, feetText, isAbility);
        }
        else
        {
            ToolTipSystem.Show(content, header, helmetText, torsoText, swordText, feetText);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        ToolTipSystem.Hide();
    }
}

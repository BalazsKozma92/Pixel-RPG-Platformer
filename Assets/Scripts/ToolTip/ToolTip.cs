using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI headerField;
    [SerializeField] TextMeshProUGUI contentField;
    [SerializeField] TextMeshProUGUI helmetField;
    [SerializeField] TextMeshProUGUI torsoField;
    [SerializeField] TextMeshProUGUI swordField;
    [SerializeField] TextMeshProUGUI feetField;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] int characterWrapLimit;

    public void SetText(string content, string header, string helmet, string torso, string sword, string feet) {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;    

        layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;

        if (string.IsNullOrEmpty(header)) {
            headerField.gameObject.SetActive(false);
        } else {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        helmetField.text = helmet;
        torsoField.text = torso;
        swordField.text = sword;
        feetField.text = feet;
    }

    public void SetText(string content, string header, string helmet, string torso, string sword, string feet, bool isAbility) {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;    

        layoutElement.enabled = headerLength > characterWrapLimit || contentLength > characterWrapLimit;

        if (string.IsNullOrEmpty(header)) {
            headerField.gameObject.SetActive(false);
        } else {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;
        helmetField.text = helmet;
        torsoField.text = torso;
        swordField.text = sword;
        feetField.text = feet;
    }
}

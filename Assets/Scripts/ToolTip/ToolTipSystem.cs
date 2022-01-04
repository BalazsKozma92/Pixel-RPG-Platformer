using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    private static ToolTipSystem current;

    [SerializeField] ToolTip toolTip;

    public void Awake() {
        current = this;
    }

    public static void Show(string content, string header, string helmet, string torso, string sword, string feet) {
        current.toolTip.SetText(content, header, helmet, torso, sword, feet);
        current.toolTip.gameObject.SetActive(true);
    }

    public static void Show(string content, string header, string helmet, string torso, string sword, string feet, bool isAbility) {
        current.toolTip.SetText(content, header, helmet, torso, sword, feet);
        current.toolTip.gameObject.SetActive(true);
    }

    public static void Hide() {
        current.toolTip.gameObject.SetActive(false);
    }
}

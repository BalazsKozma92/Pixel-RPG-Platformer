using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hovering : MonoBehaviour
{
    [SerializeField] float upAndDownValue;
    [SerializeField] float duration;
    float currentY;

    void Start()
    {
        currentY = transform.position.y;
        if (transform)
        {
            transform.DOMoveY(currentY + upAndDownValue, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }
    }
}

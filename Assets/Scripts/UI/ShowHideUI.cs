using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField] float duration;

    CanvasGroup canvasGroup;
    Tween fadeTween;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (canvasGroup.alpha == 0)
            {
                // PlayerBase.Instance.Freeze(true);
                AudioPlayer.Instance.PlayUIOpen();
                FadeIn(duration);
            }
            else if (canvasGroup.alpha == 1)
            {
                // PlayerBase.Instance.Freeze(false);
                AudioPlayer.Instance.PlayUIClose();
                FadeOut(duration);
            }
        }
    }

    void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = canvasGroup.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn(float duration)
    {
        Fade(1f, duration, () =>
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(0f, duration, () =>
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }
}
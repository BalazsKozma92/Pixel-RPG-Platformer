using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class ShowHideUI : MonoBehaviour
{
    [SerializeField] float duration;

    CanvasGroup canvasGroup;
    PlayerInputHandler playerInputHandler;
    Tween fadeTween;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        playerInputHandler = FindObjectOfType<PlayerInputHandler>();
    }

    void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (canvasGroup.alpha == 0)
            {
                playerInputHandler.FreezePlayer(true);
                AudioPlayer.Instance.PlayUIOpen();
                FadeIn(duration);
            }
            else if (canvasGroup.alpha == 1)
            {
                playerInputHandler.FreezePlayer(false);
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
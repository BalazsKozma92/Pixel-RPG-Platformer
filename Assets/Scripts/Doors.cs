using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Doors : MonoBehaviour
{
    [SerializeField] Vector2 endCoordinates;
    [SerializeField] CanvasGroup screenCanvas;
    [SerializeField] float duration;

    bool playerIsHere = false;
    Tween fadeTween;
    
    private void Update()
    {
        if (playerIsHere && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (screenCanvas.alpha == 0)
            {
                FadeOut(duration);
                Invoke("FadeIn", duration);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }

    void Fade(float endValue, TweenCallback onEnd)
    {
        if (fadeTween != null)
        {
            fadeTween.Kill(false);
        }

        fadeTween = screenCanvas.DOFade(endValue, duration);
        fadeTween.onComplete += onEnd;
    }

    public void FadeIn()
    {
        // Player.Instance.transform.position = endCoordinates;
        Fade(0f, () =>
        {
            screenCanvas.interactable = true;
            screenCanvas.blocksRaycasts = true;
        });
    }

    public void FadeOut(float duration)
    {
        Fade(1f, () =>
        {
            screenCanvas.interactable = false;
            screenCanvas.blocksRaycasts = false;
        });
    }
}

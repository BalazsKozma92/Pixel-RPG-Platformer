using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class UnlockGem : MonoBehaviour
{
    [SerializeField] string gemName;
    [SerializeField] Volume globalVolume;
    [SerializeField] TextMeshProUGUI accouncementText;

    ChromaticAberration ca;
    LensDistortion ld;
    GameObject coverToDestroy;

    private void Start() {
        coverToDestroy = GameObject.Find(gemName);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            GameObject.Find(gemName).GetComponentInParent<ToolTipTrigger>().enabled = true;
            Destroy(coverToDestroy);

            globalVolume.profile.TryGet(out ca);
            globalVolume.profile.TryGet(out ld);
            Invoke("GemPickupEffect", .01f);

            AudioPlayer.Instance.PlayGemAcquired();
            accouncementText.text = "Gem acquired: " + gemName;
            accouncementText.GetComponent<Animator>().SetTrigger("Appear");

            gameObject.SetActive(false);
        }
    }

    void GemPickupEffect()
    {
        ca.intensity.value = 1;
        ld.intensity.value = -.2f;
        Invoke("GemPickupEffect2", .35f);
    }

    void GemPickupEffect2()
    {
        ca.intensity.value = 0;
        ld.intensity.value = 0;
    }
}

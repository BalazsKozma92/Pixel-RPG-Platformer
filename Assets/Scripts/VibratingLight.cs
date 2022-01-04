using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class VibratingLight : MonoBehaviour
{
    [SerializeField] float flickerTime;
    float baseInnerRadius;
    float baseOuterRadius;
    Light2D lightComponent;

    private void Start() {
        lightComponent = GetComponent<Light2D>();
        baseInnerRadius = lightComponent.pointLightInnerRadius;
        baseOuterRadius = lightComponent.pointLightOuterRadius;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float changeInRadius = Random.Range(-.08f, .08f);
            lightComponent.pointLightInnerRadius = baseInnerRadius + changeInRadius;
            lightComponent.pointLightOuterRadius = baseOuterRadius + changeInRadius;
            yield return new WaitForSeconds(Random.Range(flickerTime - .05f, flickerTime + .05f));
        }
    }
}

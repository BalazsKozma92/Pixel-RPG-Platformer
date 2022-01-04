using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] float originalShakeDuration = .2f;
    [SerializeField] float originalShakeMagnitude = .5f;

    float shakeDuration;
    float shakeMagnitude;
    
    CinemachineVirtualCamera cinemachine;
    CinemachineFramingTransposer composer;
    Vector3 initialPos;

    private void Awake() {
        cinemachine = GetComponent<CinemachineVirtualCamera>();
        composer = cinemachine.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Start() {
        initialPos = composer.m_TrackedObjectOffset;
    }

    public void Play() {
        shakeDuration = originalShakeDuration;
        shakeMagnitude = originalShakeMagnitude;
        StartCoroutine(Shake());
    }

    public void Play(float shakeDur, float shakeMagn) {
        shakeDuration = shakeDur;
        shakeMagnitude = shakeMagn;
        StartCoroutine(Shake());
    }

    IEnumerator Shake() {
        float elapsedTime = 0;
        while (elapsedTime < shakeDuration) {
            Vector3 shakePos = initialPos + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            composer.m_TrackedObjectOffset = shakePos;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        composer.m_TrackedObjectOffset = initialPos;
    }
}

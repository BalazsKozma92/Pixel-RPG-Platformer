using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float parallaxEffectMultiplier;
    [SerializeField] bool shouldRepeat;

    Transform cameraTransform;
    float startPos;
    float backgroundLength;

    private void Awake() {
        cameraTransform = Camera.main.transform;
    }

    private void Start() {
        startPos = transform.position.x;
        backgroundLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate() {
        float temp = (cameraTransform.position.x * (1 - parallaxEffectMultiplier));
        float distance = ((cameraTransform.position.x - startPos) * parallaxEffectMultiplier);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (shouldRepeat)
        {
            if (temp > startPos + backgroundLength)
            {
                startPos += backgroundLength;
            }
            else if (temp < startPos - backgroundLength)
            {
                startPos -= backgroundLength;
            }
        }
    }
}

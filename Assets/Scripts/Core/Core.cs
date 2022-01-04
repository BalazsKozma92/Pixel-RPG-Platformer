using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement { get; private set; }

    void Awake()
    {
        Movement = GetComponentInChildren<Movement>();

        if (!Movement) { Debug.LogError("Missing core component"); }    
    }
}

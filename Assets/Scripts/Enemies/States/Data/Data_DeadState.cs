using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/Dead state")]
public class Data_DeadState : ScriptableObject
{
    public GameObject deathChunkParticles;
    public GameObject deathBloodParticles;
}

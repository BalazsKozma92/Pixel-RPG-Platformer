using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newIdleStateData", menuName = "Data/State Data/Idle state")]
public class Data_IdleState : ScriptableObject
{
    public float minIdleTime = 1f;
    public float maxIdleTime = 2.5f;
}

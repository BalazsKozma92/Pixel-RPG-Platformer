using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/State Data/Dodge state")]
public class Data_DodgeState : ScriptableObject
{
    public float dodgeSpeed = 10f;
    public Vector2 dodgeAngle;
    public float dodgeTime = .3f;
    public float dodgeCooldown = 2f;
}

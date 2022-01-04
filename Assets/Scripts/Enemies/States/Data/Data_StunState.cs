using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun state")]
public class Data_StunState : ScriptableObject
{
    public float stunTime = 3f;
    public float stunKnockbackTime = .2f;
    public float stunKnockbackSpeed = 14f;
    public Vector2 stunKnockbackAngle;
}

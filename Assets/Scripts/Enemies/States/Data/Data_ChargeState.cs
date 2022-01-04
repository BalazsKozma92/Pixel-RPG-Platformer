using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newChargeStateData", menuName = "Data/State Data/Charge state")]
public class Data_ChargeState : ScriptableObject
{
    public float chargeSpeed = 8f;
    public float chargeTime = .6f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PogoData : ScriptableObject
{
    public int Bounciness;
    public float SlamForce;
    public float SlamForceEnergyLoss;
    public float RotationSpeed;
}

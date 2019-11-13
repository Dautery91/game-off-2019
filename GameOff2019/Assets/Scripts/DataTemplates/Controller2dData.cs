using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Controller2dData : ScriptableObject
{
    public int HorizontalRayCount = 4;
    public int VerticalRayCount = 4;

    public float HorizontalSpeed = 5f;

    public float tileLength = 1f;

    // @VJS Looks like this isnt used, right?
    public float apexTimePerTile = 1f;

    public float maxClimbableSlope = 75f;
    public float maxDescendableSlope = 75f;

    public float HangTimeDuration = 1.5f;
    public int StartingJumpStrength = 3;

    public int NormalJumpHeight = 1;





}

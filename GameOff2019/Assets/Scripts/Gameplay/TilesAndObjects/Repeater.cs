using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Repeater : WireTile
{
    public GameObject onSprite;
    public GameObject OffSprite;

    public float skinWidth = 0.015f;

    public LayerMask collideableLayer;

    GridCollisionFlags gridCollisionFlags;

    BoxCollider2D collider;
    RayCastOrigins rayCastOrigins;

    int collisionCount = 0;
    Tilemap tilemap;

    float tileLength;

    private bool elecSoundOn = false;
}

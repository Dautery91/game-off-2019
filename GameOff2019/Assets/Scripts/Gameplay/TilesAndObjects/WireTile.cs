using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class WireTile : IObject
{

    public GameObject onSprite;
    public GameObject offSprite;


    public override void TurnOff(){
        onSprite.SetActive(false);
        offSprite.SetActive(true);
    }
    public override void TurnOn(){
        onSprite.SetActive(true);
        offSprite.SetActive(false);
    }

    
}

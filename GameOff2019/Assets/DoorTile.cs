using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTile : IObject
{

    public GameObject doorSprite;

    public override void TurnOn(){
        doorSprite.SetActive(true);
    }

    public override void TurnOff(){
        doorSprite.SetActive(false);
    }
    
}

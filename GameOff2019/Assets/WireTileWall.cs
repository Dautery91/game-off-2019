using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WireTileWall : WireTile
{
    public bool Solid;
    public GameObject solidOnSprite;
    public GameObject solidOffSprite;

    void ToggleSolidness(){
        Solid = !Solid;
    }

    void ResetSprites(){
        solidOffSprite.SetActive(false);
        solidOnSprite.SetActive(false);
        onSprite.SetActive(false);
        offSprite.SetActive(false);
    }

    protected override void InitializeState(){
        ResetSprites();
        if(Solid){
            if(objectState == ObjectState.On){
                solidOnSprite.SetActive(true);
            }
            else{
                solidOffSprite.SetActive(true);
            }
            
        }
        else{
            if(objectState == ObjectState.On){
                onSprite.SetActive(true);
            }
            else{
                offSprite.SetActive(true);
            }
            
        }
    }

    public override void TurnOff(){
        ToggleSolidness();
        ResetSprites();
        if(Solid){
            solidOffSprite.SetActive(true);
        }
        else{
            offSprite.SetActive(true);
        }
    }

    public override void TurnOn(){

        ToggleSolidness();
        ResetSprites();
        if(Solid){
            solidOnSprite.SetActive(true);
        }
        else{
            onSprite.SetActive(true);
        }

    }
}

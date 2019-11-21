using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Launchpad : IObject
{
    [SerializeField] GameObject OnSprite;
    [SerializeField] GameObject OffSprite;

    BoxCollider2D boxCollider;

    public override void TurnOn(){

        OnSprite.SetActive(true);
        OffSprite.SetActive(false);

    }

    public override void TurnOff(){

        OnSprite.SetActive(false);
        OffSprite.SetActive(true);

    }

    
    
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 direction;

        if(other.transform.position.y>transform.position.y){
            direction = Vector2.up;
        }
        else if(other.transform.position.y<transform.position.y)
        {
            direction = Vector2.up*-1;
        }
        else{
            if(other.transform.position.x<transform.position.x){
                direction = Vector2.right*-1;
            }
            else{
                direction = Vector2.right;
            }
        }

        if(objectState==ObjectState.On){
            if(other.gameObject.tag=="Player"){

                GridController2D controller2D = other.gameObject.GetComponent<GridController2D>();
                if(controller2D!=null&&!controller2D.launched){
                    controller2D.Launch(direction);
                }

            }

            // if(other.gameObject.tag=="Block"){

            //     gridBlockController2D controller2D = other.gameObject.GetComponent<gridBlockController2D>();
            //     if(controller2D!=null&&!controller2D.launched){
            //         controller2D.Launch(direction);
            //     }

            // }
        }
    }
    
    
}

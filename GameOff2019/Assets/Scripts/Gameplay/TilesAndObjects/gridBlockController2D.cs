using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gridBlockController2D : GridController2DBase
{
    
    public override void Start()
    {
        base.Start();

    }

   
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

        GetCollisions();


        if(!moving&&!launched&&!trapped){


            GravityMovement();


        }

        
    }

    
   

   


    public void HorizontalMovement(float horizontalMovement)
    {
        if(gridCollisionFlags.below&&!moving&&!trapped){
           

            Vector3Int newTile = currentTile;

            if(horizontalMovement<0&&gridCollisionFlags.lslopeAngle<maxSlopeAngle){
                newTile = new Vector3Int(currentTile.x-1,currentTile.y,0);
                if(gridCollisionFlags.lslopeAngle>0){
                    newTile.y+=1;
                }
                else if(gridCollisionFlags.dslopeAngle>0){
                    newTile.y-=1;
                }
               
            }

            if(horizontalMovement>0&&gridCollisionFlags.rslopeAngle<maxSlopeAngle){
                newTile = new Vector3Int(currentTile.x+1,currentTile.y,0);
                if(gridCollisionFlags.rslopeAngle>0){
                    newTile.y+=1;
                }
                else if(gridCollisionFlags.dslopeAngle>0){
                    newTile.y-=1;
                }
            }

            //edge case for top of a slope
            if(tilemap.HasTile(newTile)){
                newTile.y+=1;
            }


            
            StartCoroutine(SmoothMove(newTile));

        }
    }

    private void GravityMovement()
    {
        if(!gridCollisionFlags.below){
            int distanceIntiles = (int)(GetDistanceToCollideAbleTile(Vector2.up*-1)/tilelength);
            

            Vector3Int newTile = new Vector3Int(currentTile.x,currentTile.y-distanceIntiles,0);

            StartCoroutine(SmoothMove(newTile));
            
        }
    }


}

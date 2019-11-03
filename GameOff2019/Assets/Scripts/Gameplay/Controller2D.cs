using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct RayCastBounds{
    public Vector2 topLeft,topRight;
    public Vector2 bottomLeft,bottomRight;
}

public struct CollisionFlags{
    public bool above,below,left,right;

    public void Reset(){
        above = below = false;
        left = right = false;
    }
}


[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    const float skinWidth = 0.015f;

    [SerializeField] Controller2dData controller2DData;

    [SerializeField] PlayerInputReader playerInputReader;

    [SerializeField] Vector2Data gravity;

    [SerializeField] LayerMask layerMask;
    BoxCollider2D collider;

    public CollisionFlags collisionFlags;

    float VerticalRaySpacing;

    float HorizontalRaySpacing;


    RayCastBounds rayCastBounds;
    private float HorizontalRayCount;
    private float VerticalRayCount;

    float fallDistance = 0;

    int jumpCount = 0;

    Vector3 velocity;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        collisionFlags.Reset();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        // need to improve this to account for change in gravity direction
        if(collisionFlags.below){
            

            //floor or ceil or round
            jumpCount+= Mathf.RoundToInt(fallDistance/controller2DData.tileLength);
            fallDistance = 0;

           
        }
        else if(velocity.y<0)
        {
            fallDistance+=Mathf.Abs(velocity.y*Time.deltaTime);
        }

        //prevent velocity buildup on collision
        if(collisionFlags.above||collisionFlags.below){
             
            velocity.y=0;
        }
        if(collisionFlags.left||collisionFlags.right){
             
            velocity.x=0;
        }


        
        
        // should probably move this into input script
        float horizontalDisplacement = playerInputReader.HorizontalMoveInput*controller2DData.HorizontalSpeed;
        velocity.x = horizontalDisplacement;
        
        if(playerInputReader.JumpInput && collisionFlags.below){
            float maxHeight = ((float)jumpCount)*controller2DData.tileLength+collider.bounds.size.y/4;
            velocity.y += Mathf.Sqrt( maxHeight*Mathf.Abs(gravity.data.y)*2);
            jumpCount = 0;

        }
        
        velocity += new Vector3(gravity.data.x*Time.deltaTime,gravity.data.y*Time.deltaTime,0);
        
      

        Move(velocity*Time.deltaTime);
        

    }

    void Move(Vector3 movement){
        collisionFlags.Reset();

        UpdateRayCastBounds();

        if(Mathf.Abs(movement.x)>0){
            HorizontalCollisionDetection(ref movement);
        }

        if(Mathf.Abs(movement.y)>0){
            VerticalCollisionDetection(ref movement);
        }
        
       
        
        transform.Translate(movement);
    }

    void VerticalCollisionDetection(ref Vector3 movement){

        float direction = Mathf.Sign(movement.y);
       
        float raydistance = Mathf.Abs(movement.y)+skinWidth;

       

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (direction==1?rayCastBounds.topLeft:rayCastBounds.bottomLeft);
            rayOrigin.x += VerticalRaySpacing*i + movement.x;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin,direction*Vector2.up,raydistance,layerMask);

            Debug.DrawRay(rayOrigin,Vector2.up*direction,Color.red);

            if(hit){
              

                movement.y = direction*(hit.distance-skinWidth);
                raydistance = hit.distance;

                collisionFlags.above = direction==1;
                collisionFlags.below = direction==-1;
            }
        }
    }

     void HorizontalCollisionDetection(ref Vector3 movement){

        float direction = Mathf.Sign(movement.x);
        
        float raydistance = Mathf.Abs(movement.x)+skinWidth;

       

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (direction==1?rayCastBounds.bottomRight:rayCastBounds.bottomLeft);
            rayOrigin.y += HorizontalRaySpacing*i;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin,direction*Vector2.right,raydistance,layerMask);

            Debug.DrawRay(rayOrigin,Vector2.right*direction,Color.red);

            if(hit){
                
                float slopeAngle = Vector2.Angle(hit.normal,Vector2.up);
                if(i==0 && slopeAngle<= controller2DData.maxClimbableSlope){
                    ClimbSlope(ref movement,slopeAngle);
                    break;
                }

                movement.x = direction*(hit.distance-skinWidth);
                raydistance = hit.distance;
                collisionFlags.right = direction==1;
                collisionFlags.left = direction==-1;
            }
        }
    }

    private void ClimbSlope(ref Vector3 movement, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(movement.x);
        float CalculatedMovementY = moveDistance*Mathf.Sin(Mathf.Deg2Rad*slopeAngle);
        if(CalculatedMovementY>=movement.y){
            movement.y = moveDistance*Mathf.Sin(Mathf.Deg2Rad*slopeAngle);
            movement.x = moveDistance*Mathf.Cos(Mathf.Deg2Rad*slopeAngle)*Mathf.Sign(movement.x);
            collisionFlags.below = true;
        }
       
    }

    void UpdateRayCastBounds(){
        Bounds bounds = collider.bounds;
        bounds.Expand(-1*skinWidth);

        rayCastBounds.bottomLeft = new Vector2(bounds.min.x,bounds.min.y);
        rayCastBounds.bottomRight = new Vector2(bounds.max.x,bounds.min.y);
        rayCastBounds.topLeft = new Vector2(bounds.min.x,bounds.max.y);
        rayCastBounds.topRight = new Vector2(bounds.max.x,bounds.max.y);
    }

    void CalculateRaySpacing(){
        Bounds bounds = collider.bounds;
        bounds.Expand(-1*skinWidth);

        HorizontalRayCount = Mathf.Clamp(controller2DData.HorizontalRayCount,2,int.MaxValue);
        VerticalRayCount = Mathf.Clamp(controller2DData.HorizontalRayCount,2,int.MaxValue);


        HorizontalRaySpacing = bounds.size.x/(HorizontalRayCount-1);
        VerticalRaySpacing = bounds.size.x/(VerticalRayCount-1);

    }
}

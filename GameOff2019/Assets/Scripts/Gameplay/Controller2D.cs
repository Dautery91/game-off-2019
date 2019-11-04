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
    public bool climbingSlope;
    public bool descendingSlope;
    public float slopeAngle, slopeAngleOld;

    public void Reset()
    {
        above = below = false;
        left = right = false;
        climbingSlope = false;
        descendingSlope = false;

        slopeAngleOld = slopeAngle;
        slopeAngle = 0;
    }
}


[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    #region Fields

    const float skinWidth = 0.015f;

    [SerializeField] Controller2dData controller2DData;

    [SerializeField] PlayerInputReader playerInputReader;

    [SerializeField] Vector2Data gravity;

    [SerializeField] LayerMask collisionMask;
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

    // Hang time support
    bool atJumpPeak = false;
    float currentYVelocity;
    float oldYVelocity;
    Timer jumpApexTimer;

    // Jump Count Support
    float heightOnJump;
    float heightOnLand;
    bool haveJumped = false;

    #endregion



    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        collisionFlags.Reset();
        jumpCount = controller2DData.StartingJumpStrength;
        jumpApexTimer = this.gameObject.AddComponent<Timer>();
        jumpApexTimer.Duration = controller2DData.HangTimeDuration;
        atJumpPeak = false;

        UpdateJumpCountHUD();
    }

    void Update()
    {
        if (oldYVelocity >= 0 && currentYVelocity < 0 && haveJumped)
        {
            atJumpPeak = true;
        }
        else if (jumpApexTimer.Finished && atJumpPeak)
        {
            atJumpPeak = false;
            jumpApexTimer.Stop();
        }

        oldYVelocity = currentYVelocity;

        CalculateFallDistance();

        CalculateVelocityOnCollision();

        HandleHorizontalInput();

        CalculateGravityMovement();

        HandleJumpInput();

        // Moves based on Vector2D "velocity" calculated using the above methods
        Move(velocity * Time.deltaTime);


    }

    private void CalculateFallDistance()
    {
        // need to improve this to account for change in gravity direction
        //@DAUTERY I am removing havejumped from the condition below as the player can accumulate jumpcount with jumping
        //by just falling
        if (collisionFlags.below)
        {

            //floor or ceil or round
            jumpCount += Mathf.RoundToInt(fallDistance / controller2DData.tileLength);

            UpdateJumpCountHUD();

            fallDistance = 0;
            haveJumped = false;

        }
        //falling
        if (velocity.y < 0 && !collisionFlags.below && !atJumpPeak)
        {
            fallDistance += Mathf.Abs(velocity.y * Time.deltaTime);
        }
    }

    private void CalculateVelocityOnCollision()
    {
        //prevent velocity buildup on collision
        if (collisionFlags.above || collisionFlags.below)
        {
            velocity.y = 0;
        }
        if (collisionFlags.left || collisionFlags.right)
        {

            velocity.x = 0;
        }
    }

    private void HandleHorizontalInput()
    {
        if (collisionFlags.below || atJumpPeak)
        {
            
            float horizontalDisplacement = playerInputReader.HorizontalMoveInput * controller2DData.HorizontalSpeed;
            velocity.x = horizontalDisplacement;
            //@DAUTERY shouldn't we start this when we first set atJumpPeak = true
            if (atJumpPeak && !jumpApexTimer.Running)
            {
                jumpApexTimer.Run();
            }
        }

    }

    private void CalculateGravityMovement()
    {
        velocity += new Vector3(gravity.data.x * Time.deltaTime, gravity.data.y * Time.deltaTime, 0);
    }

    private void HandleJumpInput()
    {
        if (playerInputReader.JumpInput && collisionFlags.below)
        {
            float maxHeight = ((float)jumpCount) * controller2DData.tileLength + collider.bounds.size.y / 4;
            velocity.y += Mathf.Sqrt(maxHeight * Mathf.Abs(gravity.data.y) * 2);
            jumpCount = 0;
            haveJumped = true;
            UpdateJumpCountHUD();

        }
    }

    void Move(Vector3 movement)
    {
        collisionFlags.Reset();
        UpdateRayCastBounds();

        if (movement.y < 0)
        {
            DescendSlope(ref movement);
        }

        if(Mathf.Abs(movement.x) > 0)
        {
            HorizontalCollisionDetection(ref movement);
        }

        if(Mathf.Abs(movement.y) > 0)
        {
            VerticalCollisionDetection(ref movement);
        }

        currentYVelocity = movement.y;

        if (!collisionFlags.below && !atJumpPeak)
        {
            movement.x = 0;
        }

        if (!collisionFlags.below && atJumpPeak)
        {
            movement.y = 0;
        }

        transform.Translate(movement);
    }

    void VerticalCollisionDetection(ref Vector3 movement)
    {

        float direction = Mathf.Sign(movement.y);
       
        float raydistance = Mathf.Abs(movement.y) + skinWidth;

       

        for (int i = 0; i < VerticalRayCount; i++)
        {
            Vector2 rayOrigin = (direction == 1 ? rayCastBounds.topLeft : rayCastBounds.bottomLeft);
            rayOrigin.x += VerticalRaySpacing * i + movement.x;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction * Vector2.up, raydistance, collisionMask);

            Debug.DrawRay(rayOrigin,Vector2.up * direction,Color.red);

            if(hit)
            {

                movement.y = direction * (hit.distance-skinWidth);
                raydistance = hit.distance;

                if (collisionFlags.climbingSlope)
                {
                    movement.x = movement.y / Mathf.Tan(collisionFlags.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(movement.x);
                }

                collisionFlags.above = direction==1;
                collisionFlags.below = direction==-1;
            }
        }
    }

     void HorizontalCollisionDetection(ref Vector3 movement)
    {
        float direction = Mathf.Sign(movement.x);
        
        float raydistance = Mathf.Abs(movement.x)+skinWidth;

        for (int i = 0; i < HorizontalRayCount; i++)
        {
            Vector2 rayOrigin = (direction==1?rayCastBounds.bottomRight:rayCastBounds.bottomLeft);
            rayOrigin.y += HorizontalRaySpacing*i;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin,direction*Vector2.right,raydistance,collisionMask);

            Debug.DrawRay(rayOrigin,Vector2.right*direction,Color.red);

            if(hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal,Vector2.up);

                if(i==0 && slopeAngle <= controller2DData.maxClimbableSlope)
                {
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisionFlags.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        movement.x -= distanceToSlopeStart * direction;
                    }

                    ClimbSlope(ref movement, slopeAngle);
                    velocity.x += distanceToSlopeStart * direction;
                    // @VJS Is this break needed?
                    //@DAUTERY not anymore as you added the climbing slope flag
                    //break;
                }

                if (!collisionFlags.climbingSlope || slopeAngle > controller2DData.maxClimbableSlope)
                {
                    movement.x = direction * (hit.distance - skinWidth);
                    raydistance = hit.distance;

                    if (collisionFlags.climbingSlope)
                    {
                        movement.y = Mathf.Tan(collisionFlags.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(movement.x);
                    }

                    collisionFlags.right = direction == 1;
                    collisionFlags.left = direction == -1;
                }
            }
        }
    }

    // Makes move speed on slope the same as move speed on flat surface
    private void ClimbSlope(ref Vector3 movement, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(movement.x);
        float calculatedClimbMovementY = moveDistance * Mathf.Sin(Mathf.Deg2Rad * slopeAngle);

        if(calculatedClimbMovementY >= movement.y)
        {
            movement.y = moveDistance * Mathf.Sin(Mathf.Deg2Rad*slopeAngle);
            movement.x = moveDistance * Mathf.Cos(Mathf.Deg2Rad*slopeAngle) * Mathf.Sign(movement.x);
            collisionFlags.below = true;
            collisionFlags.climbingSlope = true;
            collisionFlags.slopeAngle = slopeAngle;
        }
       
    }

    private void DescendSlope(ref Vector3 movement)
    {
        float direction = Mathf.Sign(movement.x);
        Vector2 rayOrigin = (direction == -1) ? rayCastBounds.bottomRight : rayCastBounds.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= controller2DData.maxDescendableSlope)
            {
                if (Mathf.Sign(hit.normal.x) == direction)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(movement.x)) 
                    {
                        float moveDistance = Mathf.Abs(movement.x);
                        float calculatedDescendMovementY = moveDistance * Mathf.Sin(Mathf.Deg2Rad * slopeAngle);
                        movement.x = moveDistance * Mathf.Cos(Mathf.Deg2Rad * slopeAngle) * Mathf.Sign(movement.x);
                        movement.y -= calculatedDescendMovementY;

                        collisionFlags.slopeAngle = slopeAngle;
                        collisionFlags.descendingSlope = true;
                        collisionFlags.below = true;
                    }
                }
            }
        }
    }


    void UpdateRayCastBounds()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(-1 * skinWidth);
        
        rayCastBounds.bottomLeft = new Vector2(bounds.min.x,bounds.min.y);
        rayCastBounds.bottomRight = new Vector2(bounds.max.x,bounds.min.y);
        rayCastBounds.topLeft = new Vector2(bounds.min.x,bounds.max.y);
        rayCastBounds.topRight = new Vector2(bounds.max.x,bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(-1*skinWidth);

        HorizontalRayCount = Mathf.Clamp(controller2DData.HorizontalRayCount,2,int.MaxValue);
        VerticalRayCount = Mathf.Clamp(controller2DData.HorizontalRayCount,2,int.MaxValue);

        // @VJS - I changed horizontal ray spacing to be bounds.size.Y (you had x) per the tutorial
        HorizontalRaySpacing = bounds.size.y/(HorizontalRayCount-1);
        VerticalRaySpacing = bounds.size.x/(VerticalRayCount-1);

    }

    private void UpdateJumpCountHUD()
    {
        // Raise event to display jump count to HUD
        EventParam jumpParam = new EventParam();
        jumpParam.intParam = jumpCount;
        EventManager.RaiseEvent(EventNames.JumpUpdateEvent, jumpParam);
    }
}

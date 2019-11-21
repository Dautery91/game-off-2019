using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public struct RayCastOrigins{
    public Vector2 up,down;
    public Vector2 left,right;
    
}
public struct GridCollisionFlags{
    public bool above,below,left,right;

    public float lslopeAngle , rslopeAngle, dslopeAngle;

    public Collider2D Cabove,Cbelow,Cleft,Cright;

    public void Reset()
    {
        above = below = false;
        left = right = false;

        Cabove = Cbelow = Cleft = Cright = null;

        dslopeAngle = lslopeAngle = rslopeAngle = 0;

    }
}

[RequireComponent(typeof(BoxCollider2D))]
public class GridController2D : MonoBehaviour
{

    [SerializeField] PlayerInputReader playerInputReader;

    [SerializeField] Tilemap tilemap;

    [SerializeField] IntData jumpCount;

    BoxCollider2D collider;

    RayCastOrigins rayCastOrigins;

    const float skinWidth = 0.015f;

    public GridCollisionFlags gridCollisionFlags;

    float tilelength;

    Vector3Int currentTile;

    bool moving = false;

    bool hanging = false;

    public FloatData HorizontalMovementSpeedData;
    public FloatData VerticalMovementSpeedData;

    private float horizontalMovementSpeed;

    private float verticalMovementSpeed;

    public int horizontalTileMovementDuringHanging = 2;

    public float maxSlopeAngle = 80f;

    public LayerMask collideableLayer;

    Timer HangingTimer;

    [HideInInspector]
    public bool launched = false;

    // Animation support fields
    [SerializeField] Animator animator;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        tilelength = tilemap.cellSize.x;
        currentTile = tilemap.WorldToCell(transform.position);

        horizontalMovementSpeed = HorizontalMovementSpeedData.data;
        verticalMovementSpeed = VerticalMovementSpeedData.data;

        collider = GetComponent<BoxCollider2D>();
        
        
        //move into cell centre if not already there
        transform.position = tilemap.CellToWorld(currentTile)+tilemap.tileAnchor;

        HangingTimer = this.gameObject.AddComponent<Timer>();

        jumpCount.Data = 0;


    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

       
        GetCollisions();

        if(!moving&&!launched)
        {



            if (hanging && !HangingTimer.Running)
            {

                if (HangingTimer.Finished)
                {
                    HangingTimer.Stop();
                    hanging = false;
                }
                else
                {
                    float duration = horizontalTileMovementDuringHanging * tilelength / horizontalMovementSpeed;
                    HangingTimer.Duration = duration;
                    HangingTimer.Run();
                }

            }

            GravityMovement();

            HorizontalMovement();

            JumpMovement();

            CheckIfIdle();

        }




    }

    /// <summary>
    /// For animation
    /// </summary>
    private void CheckIfIdle()
    {
        if (!moving)
        {
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }
    }

    IEnumerator launch(Vector2 direction){

        while(moving){
            yield return null;
        }
        if(direction==Vector2.up){
            hanging = true;
        }
        else{
            hanging = false;
        }
        int distanceIntiles = (int)(GetDistanceToCollideAbleTile(direction)/tilelength);

        Vector3Int newTile = currentTile + new Vector3Int((int)direction.x*distanceIntiles,(int)direction.y*distanceIntiles,0);

        StartCoroutine(SmoothMove(newTile));

        while(moving){
            yield return null;
        }

        launched = false;


    }

    public void Launch(Vector2 direction){

       StartCoroutine(launch(direction));
       launched = true;

    }

    private void JumpMovement()
    {

        if(gridCollisionFlags.below && playerInputReader.JumpInput){
            
            int distanceIntiles = (int)(GetDistanceToCollideAbleTile(Vector2.up)/tilelength);

            distanceIntiles = Mathf.Min(distanceIntiles,jumpCount.Data);

            animator.SetInteger("JumpStrength", jumpCount.Data);

            jumpCount.Data = 0;

            Vector3Int newTile = new Vector3Int(currentTile.x,currentTile.y+distanceIntiles,0);

            //animator.SetTrigger("JumpTrigger");
            animator.SetBool("HasJumped", true);

            StartCoroutine(SmoothMove(newTile));

            hanging = true;

            
        }

    }

    private void HorizontalMovement()
    {
        if(gridCollisionFlags.below || hanging){
            float horizontalMovement = playerInputReader.HorizontalMoveInput;

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


            if((gridCollisionFlags.Cleft!=null&&gridCollisionFlags.Cleft.tag == "Block")&&horizontalMovement<0){
                gridBlockController2D blockController2D = gridCollisionFlags.Cleft.GetComponent<gridBlockController2D>();
                if(blockController2D!=null){
                    blockController2D.HorizontalMovement(horizontalMovement);
                }
            }

            if((gridCollisionFlags.Cright!=null&&gridCollisionFlags.Cright.tag == "Block")&&horizontalMovement>0){
                gridBlockController2D blockController2D = gridCollisionFlags.Cright.GetComponent<gridBlockController2D>();
                if(blockController2D!=null){
                    blockController2D.HorizontalMovement(horizontalMovement);
                }
            }

        }
    }

    private void GravityMovement()
    {
        if (!gridCollisionFlags.below && !hanging) {
            int distanceIntiles = (int)(GetDistanceToCollideAbleTile(Vector2.up * -1) / tilelength);

            jumpCount.Data += distanceIntiles;

            Vector3Int newTile = new Vector3Int(currentTile.x, currentTile.y - distanceIntiles, 0);

            StartCoroutine(SmoothMove(newTile));

        }

    }

    void GetCollisions(){
        
        gridCollisionFlags.Reset();
        CalculateRayOrigins();

        float rayDistance = skinWidth+tilelength/2;
        
        //up
        RaycastHit2D hit;

        hit  = Physics2D.Raycast(rayCastOrigins.up,Vector2.up,rayDistance,collideableLayer);

        if(hit){

            gridCollisionFlags.above = true;
            gridCollisionFlags.Cabove = hit.collider;

            //if (hanging)
            //{
            //    animator.SetTrigger("HeadBonk");
            //}

        }


        //down
        hit  = Physics2D.Raycast(rayCastOrigins.down,-1*Vector2.up,rayDistance,collideableLayer);
        Debug.DrawRay(rayCastOrigins.down,-1*Vector2.up,Color.red);

        if(hit){

            gridCollisionFlags.below = true;
            gridCollisionFlags.Cbelow = hit.collider;
            gridCollisionFlags.dslopeAngle = Vector2.Angle(hit.normal,Vector2.up);



        }

        //left

        hit  = Physics2D.Raycast(rayCastOrigins.left,-1*Vector2.right,rayDistance,collideableLayer);

        if(hit){

            gridCollisionFlags.left = true;
            gridCollisionFlags.Cleft = hit.collider;
            gridCollisionFlags.lslopeAngle = Vector2.Angle(hit.normal,Vector2.up);

        }

        //right
        hit  = Physics2D.Raycast(rayCastOrigins.right,Vector2.right,rayDistance,collideableLayer);

        if(hit){

            gridCollisionFlags.right = true;
            gridCollisionFlags.Cright = hit.collider;
            gridCollisionFlags.rslopeAngle = Vector2.Angle(hit.normal,Vector2.up);

        }


        

    }


    void CalculateRayOrigins(){
        Bounds bounds = collider.bounds;
        bounds.Expand(-1 * skinWidth);

        rayCastOrigins.up = new Vector2(bounds.min.x+bounds.size.x/2,bounds.max.y);
        rayCastOrigins.down = new Vector2(bounds.min.x+bounds.size.x/2,bounds.min.y);
        rayCastOrigins.left = new Vector2(bounds.min.x,bounds.min.y+bounds.size.y/2);
        rayCastOrigins.right = new Vector2(bounds.max.x,bounds.min.y+bounds.size.y/2);


    }

    float GetDistanceToCollideAbleTile(Vector2 direction){

        Vector2 origin = transform.position;

        if(direction == Vector2.up){
            origin = rayCastOrigins.up;
        }
        else if(direction == -1*Vector2.up){
            origin = rayCastOrigins.down;
        }
        else if(direction == Vector2.right){
            origin = rayCastOrigins.right;
        }
        else if(direction == -1*Vector2.right){
            origin = rayCastOrigins.left;
        }





        float distance = 0;

        RaycastHit2D hit2D;

        hit2D = Physics2D.Raycast(origin,direction,float.PositiveInfinity,collideableLayer);

        if(hit2D){
            return hit2D.distance;
        }

        return distance;

    }


    IEnumerator SmoothMove(Vector3Int newTile){

        moving = true;

        float movementSpeedLocal;

        Vector3 positionToMove = tilemap.CellToLocal(newTile)+tilemap.tileAnchor;
        Vector3 originPosition = tilemap.CellToLocal(currentTile)+tilemap.tileAnchor;

        if (originPosition.y != positionToMove.y && originPosition.x == positionToMove.x)
        {
            movementSpeedLocal = verticalMovementSpeed;
        }
        else
        {
            movementSpeedLocal = horizontalMovementSpeed;
        }

        // animation checks
        if (positionToMove.x > originPosition.x)
        {
            animator.SetBool("isSlidingRight", true);
        }
        else if (positionToMove.x < originPosition.x)
        {
            animator.SetBool("isSlidingLeft", true);
        }
        else if (positionToMove.y - originPosition.y < 0)
        {
            animator.SetBool("HasStartedFalling", true);
        }


        while (transform.position!=positionToMove){

            float ratio = Mathf.Abs((Mathf.Abs((transform.position-originPosition).magnitude)+ movementSpeedLocal * Time.deltaTime)/(positionToMove-originPosition).magnitude);

            transform.position = Vector3.Lerp(originPosition,positionToMove,ratio);

            // If we are moving upwards and only have one tile length left to move, start the approach peak animation
            if (positionToMove.y - transform.position.y < tilelength / 2 && positionToMove.y - transform.position.y > 0 && hanging)
            {
                //animator.SetTrigger("ApproachJumpPeak");
                animator.SetBool("HaveApproachedPeak", true);
            }
            else if (positionToMove.y < transform.position.y && Mathf.Abs(positionToMove.y - transform.position.y) < tilelength / 2)
            {
                animator.SetBool("HasLanded", true);
            }
            else if (hanging && gridCollisionFlags.above)
            {
                animator.SetTrigger("HeadBonk");
            }

            yield return null; 
        }
        currentTile = newTile;
        moving = false;

        //Reset animation flags
        animator.SetBool("isSlidingRight", false);
        animator.SetBool("isSlidingLeft", false);
        animator.SetBool("HaveApproachedPeak", false);
        animator.SetBool("HasStartedFalling", false);
        animator.SetBool("HasJumped", false);
        animator.SetBool("HasLanded", false);
        //animator.ResetTrigger("HeadBonk");



    }





    

    



    
}


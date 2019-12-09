using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Audio;





public class GridController2D : GridController2DBase
{
    #region Fields and Properties

    [SerializeField] PlayerInputReader playerInputReader;

    

    [SerializeField] IntData jumpCount;

    bool hanging = false;

    

    private bool jumpingCorrection = false;
    private int originalJumpHeightTarget = 0;
    private int originalJumpStartingPosition = 0;

    public int horizontalTileMovementDuringHanging = 2;
    private float tilesMovedDuringHangtime = 0;

    

    Timer HangingTimer;

    

    // Animation support fields
    [SerializeField] Animator animator;

    [SerializeField] GameObject JumpIndicator;
    bool jumpIndicatorInitialized = false;

    #endregion

    

    public override void Start()
    {

       base.Start();

        JumpIndicator.SetActive(false);

        MoveJumpIndicator();

       
        HangingTimer = this.gameObject.AddComponent<Timer>();

        jumpCount.Data = 0;


    }


    void Update()
    {

        GetCollisions();

        if(!moving&&!launched&&!trapped)
        {

            if (hanging)
            {

                if (HangingTimer.Finished || tilesMovedDuringHangtime >= horizontalTileMovementDuringHanging)
                {
                    HangingTimer.Stop();
                    hanging = false;
                    tilesMovedDuringHangtime = 0;
                }
                else if (!HangingTimer.Running)
                {
                    HangingTimer.Duration = 1.25f;
                    HangingTimer.Run();
                }

            }

            

            GravityMovement();

            HorizontalMovement();

            JumpMovement();

            MoveJumpIndicator();

            CheckIfIdle();

        }

    }


    
    private void MoveJumpIndicator()
    {
        if (gridCollisionFlags.below && !jumpIndicatorInitialized)
        {
            jumpIndicatorInitialized = true;
        }

        if (jumpCount.Data > 0 && jumpIndicatorInitialized && gridCollisionFlags.below)
        {
            JumpIndicator.SetActive(true);
        }
        else
        {
            JumpIndicator.SetActive(false);
        }

        JumpIndicator.transform.position = new Vector3(transform.position.x, transform.position.y + (jumpCount.Data / tilelength), transform.position.z);
    }

    /// <summary>
    /// called by magnet tile to trap the player
    /// </summary>
    /// <param name="cell"></param>

    public override void Trap(Vector3Int cell){
        
        base.Trap(cell);

        //set it back to idle animation
        CheckIfIdle();
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

    

    private void JumpMovement()
    {

        if(gridCollisionFlags.below && playerInputReader.JumpInput)
        {
            
            int distanceIntiles = (int)(GetDistanceToCollideAbleTile(Vector2.up)/tilelength);

            distanceIntiles = Mathf.Min(distanceIntiles,jumpCount.Data);

            animator.SetInteger("JumpStrength", jumpCount.Data);

            jumpCount.Data = 0;

            if (distanceIntiles != 0)
            {
                animator.SetBool("HasJumped", true);
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySound("PlayerJump");
                }
                else
                {
                    
                }
                
            }

            originalJumpStartingPosition = currentTile.y;

            Vector3Int newTile = new Vector3Int(currentTile.x,currentTile.y + distanceIntiles,0);

            originalJumpHeightTarget = newTile.y;

            StartCoroutine(SmoothMove(newTile));

            hanging = true;

            
        }

    }



    private void HorizontalMovement()
    {
        if(gridCollisionFlags.below || hanging)
        {
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

    public override IEnumerator launch(Vector2 direction){
        
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

    


    

    


    // Need to rework the animation stuff / this is messy as is
    protected override IEnumerator SmoothMove(Vector3Int newTile)
    {

        moving = true;

        float movementSpeedLocal;

        Vector3 positionToMove = tilemap.CellToLocal(newTile)+tilemap.tileAnchor;
        Vector3 originPosition = tilemap.CellToLocal(currentTile)+tilemap.tileAnchor;

        Vector3 direction = (positionToMove-originPosition).normalized;

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


        while (transform.position!=positionToMove)
        {

            Vector3Int nextTile = currentTile + new Vector3Int((int)direction.x,(int)direction.y,(int)direction.z);
            Vector3 NextTileToMovePos = tilemap.CellToLocal(nextTile)+tilemap.tileAnchor;


            while(transform.position!=NextTileToMovePos){
                float ratio = Mathf.Abs((Mathf.Abs((transform.position-originPosition).magnitude)+ movementSpeedLocal * Time.deltaTime)/(NextTileToMovePos-originPosition).magnitude);
                transform.position = Vector3.Lerp(originPosition,NextTileToMovePos,ratio);
                
                GetCollisions();
                // If we are moving upwards and only have one tile length left to move, start the approach peak animation
                if (positionToMove.y - transform.position.y < tilelength / 2 && positionToMove.y - originPosition.y > 0 && hanging)
                {
                    //animator.SetTrigger("ApproachJumpPeak");
                    animator.SetBool("HaveApproachedPeak", true);
                }
                else if (positionToMove.y<originPosition.y && (gridCollisionFlags.below||transform.position==NextTileToMovePos))
                {
                    animator.SetBool("HasLanded", true);
                    
                }
                else if (hanging && gridCollisionFlags.above)
                {
                    animator.SetTrigger("HeadBonk");
                }
                
                yield return null; 
            }


            GetCollisions();
            
            currentTile = nextTile;

        
            if(gridCollisionFlags.above||gridCollisionFlags.below){
                
                break;
            }

            
        }

        moving = false;

        if (hanging)
        {
            tilesMovedDuringHangtime += Mathf.Abs(originPosition.x - positionToMove.x);
        }

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


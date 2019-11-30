using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class gridBlockController2D : MonoBehaviour
{
    [HideInInspector]
    public Tilemap tilemap;

    BoxCollider2D boxCollider;

    RayCastOrigins rayCastOrigins;

    const float skinWidth = 0.015f;

    public GridCollisionFlags gridCollisionFlags;

    float tilelength;

    Vector3Int currentTile;

    bool moving = false;

    public FloatData HorizontalMovementSpeedData;
    public FloatData VerticalMovementSpeedData;

    private float horizontalMovementSpeed;

    private float verticalMovementSpeed;

    public float maxSlopeAngle = 80f;

    public LayerMask collideableLayer;
    public bool launched = false;

    public bool trapped = false;

    private void Awake()
    {
        Tilemap[] tilemaps = FindObjectsOfType<Tilemap>();

        foreach (Tilemap tm in tilemaps)
        {
            if (tm.tag == "Ground")
            {
                tilemap = tm;
            }
        }
    }

    void Start()
    {

        tilelength = tilemap.cellSize.x;

        horizontalMovementSpeed = HorizontalMovementSpeedData.data;
        verticalMovementSpeed = VerticalMovementSpeedData.data;

        boxCollider = GetComponent<BoxCollider2D>();


        //move into cell centre if not already there
        snapToGrid();

    }

    private void snapToGrid()
    {
        currentTile = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.CellToWorld(currentTile) + tilemap.tileAnchor;
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

    public void Trap(Vector3Int cell){
        if(moving){
            moving=false;
            StopAllCoroutines();
        }
        if(launched){
            launched = false;
        }
        currentTile = cell;
        snapToGrid();
        trapped = true;
    }

    IEnumerator launch(Vector2 direction){

        while(moving){
            yield return null;
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

    void GetCollisions(){
        
        gridCollisionFlags.Reset();
        CalculateRayOrigins();

        float rayDistance = skinWidth+tilelength/2;

        //up
        RaycastHit2D[] hits;

        hits  = Physics2D.RaycastAll(rayCastOrigins.up,Vector2.up,rayDistance,collideableLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit && hit.transform.gameObject != this.transform.gameObject)
            {

                gridCollisionFlags.above = true;
                gridCollisionFlags.Cabove = hit.collider;

            }
        }



        //down
        hits  = Physics2D.RaycastAll(rayCastOrigins.down,-1*Vector2.up,rayDistance,collideableLayer);
        Debug.DrawRay(rayCastOrigins.down,-1*Vector2.up,Color.red);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit && hit.transform.gameObject != this.transform.gameObject)
            {

                gridCollisionFlags.below = true;
                gridCollisionFlags.Cbelow = hit.collider;
                gridCollisionFlags.dslopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            }
        }

        //left

        hits  = Physics2D.RaycastAll(rayCastOrigins.left,-1*Vector2.right,rayDistance,collideableLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit && hit.transform.gameObject != this.transform.gameObject)
            {

                gridCollisionFlags.left = true;
                gridCollisionFlags.Cleft = hit.collider;
                gridCollisionFlags.lslopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            }
        }

        //right
        hits  = Physics2D.RaycastAll(rayCastOrigins.right,Vector2.right,rayDistance,collideableLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit && hit.transform.gameObject != this.transform.gameObject)
            {

                gridCollisionFlags.right = true;
                gridCollisionFlags.Cright = hit.collider;
                gridCollisionFlags.rslopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            }
        }


        

    }


    void CalculateRayOrigins(){
        Bounds bounds = boxCollider.bounds;
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

        //RaycastHit2D hit2D;

        //hit2D = Physics2D.Raycast(origin,direction,float.PositiveInfinity,collideableLayer);

        //if(hit2D){
        //    return hit2D.distance;
        //}


        RaycastHit2D[] allHits = Physics2D.RaycastAll(origin, direction, float.PositiveInfinity, collideableLayer);

        foreach (RaycastHit2D hit in allHits)
        {
            if (hit.transform.gameObject != this.transform.gameObject)
            {
                return hit.distance;
            }
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

        while (transform.position!=positionToMove){

            float ratio = Mathf.Abs((Mathf.Abs((transform.position-originPosition).magnitude)+ movementSpeedLocal * Time.deltaTime)/(positionToMove-originPosition).magnitude);

            transform.position = Vector3.Lerp(originPosition,positionToMove,ratio);

            yield return null; 
        }
        currentTile = newTile;
        moving = false;
    }




    

    

}

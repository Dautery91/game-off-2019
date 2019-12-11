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

    public float lslopeAngle, rslopeAngle, dslopeAngle;

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
public class GridController2DBase : MonoBehaviour
{
    
    protected Tilemap tilemap;

    [HideInInspector]
    public bool launched = false;

    public bool trapped = false;
    public float maxSlopeAngle = 80f;

    public LayerMask collideableLayer;

    protected BoxCollider2D boxCollider;

    protected RayCastOrigins rayCastOrigins;

    protected const float skinWidth = 0.015f;

    public GridCollisionFlags gridCollisionFlags;

    protected float tilelength;

    protected Vector3Int currentTile;

    protected bool moving = false;

    public FloatData HorizontalMovementSpeedData;
    public FloatData VerticalMovementSpeedData;
    protected float horizontalMovementSpeed;
    protected float verticalMovementSpeed;

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

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public virtual void Start()
    {
        tilelength = tilemap.cellSize.x;
        horizontalMovementSpeed = HorizontalMovementSpeedData.data;
        verticalMovementSpeed = VerticalMovementSpeedData.data;

        boxCollider = GetComponent<BoxCollider2D>();


        //move into cell centre if not already there
        snapToGrid();


    }

    protected void snapToGrid()
    {
        currentTile = tilemap.WorldToCell(transform.position);
        transform.position = tilemap.CellToWorld(currentTile) + tilemap.tileAnchor;
    }

    protected void GetCollisions(){
        
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

    public virtual void Trap(Vector3Int cell){
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

    protected float GetDistanceToCollideAbleTile(Vector2 direction){

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

    public virtual IEnumerator launch(Vector2 direction){

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

        if(trapped){
            return;
        }

       StartCoroutine(launch(direction));
       launched = true;

    }

    protected virtual IEnumerator SmoothMove(Vector3Int newTile)
    {

        moving = true;

        float movementSpeedLocal;

        Vector3 positionToMove = tilemap.CellToLocal(newTile)+tilemap.tileAnchor;
        Vector3 originPosition = tilemap.CellToLocal(currentTile)+tilemap.tileAnchor;

        Vector3 direction = (positionToMove-originPosition).normalized;

        //ramp check
        if(Mathf.Abs(direction.x)<1&&Mathf.Abs(direction.x)>0){
            direction = new Vector3(Mathf.Sign(direction.x)*1,Mathf.Sign(direction.y)*1,0);
        }

        if (originPosition.y != positionToMove.y && originPosition.x == positionToMove.x)
        {
            movementSpeedLocal = verticalMovementSpeed;
        }
        else
        {
            movementSpeedLocal = horizontalMovementSpeed;
        }


        while (transform.position!=positionToMove)
        {

            Vector3Int nextTile = currentTile + new Vector3Int((int)direction.x,(int)direction.y,(int)direction.z);
            Vector3 NextTileToMovePos = tilemap.CellToLocal(nextTile)+tilemap.tileAnchor;


            while(transform.position!=NextTileToMovePos){
                
                float ratio = Mathf.Abs((Mathf.Abs((transform.position-originPosition).magnitude)+ movementSpeedLocal * Time.deltaTime)/(NextTileToMovePos-originPosition).magnitude);
                transform.position = Vector3.Lerp(originPosition,NextTileToMovePos,ratio);
                
                yield return null; 
            }


            GetCollisions();
            
            currentTile = nextTile;

        
            if(gridCollisionFlags.above||gridCollisionFlags.below){
                
                break;
            }

            
        }

        moving = false;

    }




}

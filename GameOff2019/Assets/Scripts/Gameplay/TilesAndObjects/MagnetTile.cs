using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(BoxCollider2D))]
public class MagnetTile : WireTile
{

    public LayerMask collideableLayer;
    RayCastOrigins rayCastOrigins;

    public StringGameEvent PlayerDeathEvent;

    List<GameObject> trappedObjects;

    BoxCollider2D collider;

    Tilemap tilemap;

    float tilelength;

    public float skinWidth = 0.015f;

    struct TrapTiles{
        public Vector3Int left,right,up,down;

        public void setTiles(Vector3Int currentTile){

            left = currentTile + new Vector3Int(-1,0,0);
            right = currentTile + new Vector3Int(1,0,0);
            up = currentTile + new Vector3Int(0,1,0);
            down = currentTile + new Vector3Int(0,-1,0);
            
        }
    }

    TrapTiles trapTiles;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        trappedObjects = new List<GameObject>();
        tilemap  = this.GetComponentInParent<Tilemap>();
        collider = GetComponent<BoxCollider2D>();
        tilelength = tilemap.cellSize.x/2;
        trapTiles.setTiles(tilemap.WorldToCell(transform.position));
        CalculateRayOrigins();

    }

    void CalculateRayOrigins(){
        Bounds bounds = collider.bounds;
        bounds.Expand(-1 * skinWidth);

        rayCastOrigins.up = new Vector2(bounds.min.x+bounds.size.x/2,bounds.max.y);
        rayCastOrigins.down = new Vector2(bounds.min.x+bounds.size.x/2,bounds.min.y);
        rayCastOrigins.left = new Vector2(bounds.min.x,bounds.min.y+bounds.size.y/2);
        rayCastOrigins.right = new Vector2(bounds.max.x,bounds.min.y+bounds.size.y/2);


    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(objectState == ObjectState.On){
            GetCollisions();
        }
    }

    void GetCollisions(){

        float rayDistance = skinWidth+tilelength/2;
        
        //up
        RaycastHit2D hit;

        hit  = Physics2D.Raycast(rayCastOrigins.up,Vector2.up,rayDistance,collideableLayer);

        if(hit&&(hit.collider.tag=="Player"||hit.collider.tag=="Block")){
            TrapObject(hit.collider.gameObject,trapTiles.up);
        }


        //down
        hit  = Physics2D.Raycast(rayCastOrigins.down,-1*Vector2.up,rayDistance,collideableLayer);
        //Debug.DrawRay(rayCastOrigins.down,-1*Vector2.up,Color.red);

        if(hit&&(hit.collider.tag=="Player"||hit.collider.tag=="Block")){
            TrapObject(hit.collider.gameObject,trapTiles.down);
        }

        //left

        hit  = Physics2D.Raycast(rayCastOrigins.left,-1*Vector2.right,rayDistance,collideableLayer);

        
        if(hit&&(hit.collider.tag=="Player"||hit.collider.tag=="Block")){
            TrapObject(hit.collider.gameObject,trapTiles.left);
        }

        //right
        hit  = Physics2D.Raycast(rayCastOrigins.right,Vector2.right,rayDistance,collideableLayer);

        
        if(hit&&(hit.collider.tag=="Player"||hit.collider.tag=="Block")){
            TrapObject(hit.collider.gameObject,trapTiles.right);
        }


        

    }

    void TrapObject(GameObject gobject,Vector3Int tile){

        if(!trappedObjects.Contains(gobject)){
            trappedObjects.Add(gobject);
            if(gobject.tag=="Player"){
                gobject.GetComponent<GridController2D>().Trap(tile);
                PlayerDeathEvent.Raise("The little blob got stuck on a magnet! Try again?");
            }
            else{
                 gobject.GetComponent<gridBlockController2D>().Trap(tile);
            }
        }

    }

    void ReleaseObjects(){
        foreach(GameObject g in trappedObjects){
            if(g.tag=="Player"){
                g.GetComponent<GridController2D>().trapped = false;
            }
            else{
                 g.GetComponent<gridBlockController2D>().trapped = false;
            }
            trappedObjects.Remove(g);
        }
    }

    public override void TurnOff(){
        base.TurnOff();
        ReleaseObjects();
    }
    
}

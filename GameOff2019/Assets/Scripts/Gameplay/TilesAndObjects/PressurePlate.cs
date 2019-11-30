using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(BoxCollider2D))]
public class PressurePlate : IObject
{

    public GameObject onSprite;
    public  GameObject OffSprite;

    public float skinWidth = 0.015f;

    public LayerMask collideableLayer;

    GridCollisionFlags gridCollisionFlags;

    BoxCollider2D collider;
    RayCastOrigins rayCastOrigins;

    int collisionCount = 0;
    Tilemap tilemap;

    float tileLength;

    private bool elecSoundOn = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
       tilemap = transform.parent.GetComponent<Tilemap>();
       collider = GetComponent<BoxCollider2D>();
       tileLength = tilemap.cellSize.x;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        
        GetCollisions();
        setCollisionCount();

        ActivateWireBlocks();
        
        if(collisionCount%2 == 1 && objectState == ObjectState.Off){
           
            ToggleState();
            
        }
        else if(collisionCount%2 == 0 && objectState == ObjectState.On){
            ToggleState();
            AudioManager.instance.StopSound("ElecOn");
            elecSoundOn = !elecSoundOn;
        }
    }

    private void ActivateWireBlocks()
    {

        List<Transform> visited = new List<Transform>();
        
        Queue<WireTile> queue = new Queue<WireTile>();

        GetCollisions();

        Collider2D[] adjacentColliders = {gridCollisionFlags.Cabove,gridCollisionFlags.Cbelow,gridCollisionFlags.Cleft,gridCollisionFlags.Cright};

        foreach (var collider in adjacentColliders)
        {
            if(collider==null){
                continue;
            }
            WireTile wireTile = collider.GetComponent<WireTile>();
            if(wireTile!=null&&!visited.Contains(wireTile.transform))
            {
                queue.Enqueue(wireTile);
                if (!elecSoundOn && objectState == ObjectState.On)
                {
                    AudioManager.instance.PlaySound("ElecOn");
                    elecSoundOn = !elecSoundOn;
                }

            }

            
        }
        while(queue.Count>0){

            WireTile currentTile = queue.Peek();

            visited.Add(currentTile.transform);

            if(currentTile.objectState!=objectState){
                currentTile.ToggleState();
            }

            
            queue.Dequeue();

            GetCollisions(currentTile.GetComponent<BoxCollider2D>());

            adjacentColliders = new Collider2D[]{gridCollisionFlags.Cabove,gridCollisionFlags.Cbelow,gridCollisionFlags.Cleft,gridCollisionFlags.Cright};

            foreach (var collider in adjacentColliders)
            {
                if(collider==null){
                    continue;
                }
                WireTile wireTile = collider.GetComponent<WireTile>();
                if(wireTile!=null&&!visited.Contains(wireTile.transform)){
                    queue.Enqueue(wireTile);
                }

                
            }



        }

        

    }


    private void GetCollisions(BoxCollider2D col = null)
    {
        CalculateRayOrigins(col);
        float rayDistance = skinWidth + tileLength / 2;
        gridCollisionFlags.Reset();

        //up
        RaycastHit2D[] hits;

        hits = Physics2D.RaycastAll(rayCastOrigins.up, Vector2.up, rayDistance, collideableLayer);

        for(int i=0;i<hits.Length;i++)
        {
            if (hits[i].collider!=col)
            {

                gridCollisionFlags.above = true;
                gridCollisionFlags.Cabove = hits[i].collider;
                break;

            }
        }
        


        //down
        hits = Physics2D.RaycastAll(rayCastOrigins.down, -1 * Vector2.up, rayDistance, collideableLayer);
        //Debug.DrawRay(rayCastOrigins.down, -1 * Vector2.up, Color.red);

        for(int i=0;i<hits.Length;i++)
        {
            if (hits[i].collider!=col)
            {
                gridCollisionFlags.below = true;
                gridCollisionFlags.Cbelow = hits[i].collider;
                break;
            }


        }

        //left

        hits = Physics2D.RaycastAll(rayCastOrigins.left, -1 * Vector2.right, rayDistance, collideableLayer);

        for(int i=0;i<hits.Length;i++)
        {
            if (hits[i].collider!=col)
            {
                gridCollisionFlags.left = true;
                gridCollisionFlags.Cleft = hits[i].collider;
                break;

            }
        }

        //right
        hits = Physics2D.RaycastAll(rayCastOrigins.right, Vector2.right, rayDistance, collideableLayer);
        //Debug.DrawRay(rayCastOrigins.right,  Vector2.right, Color.red);
        for(int i=0;i<hits.Length;i++)
        {
            if (hits[i].collider!=col)
            {
                gridCollisionFlags.right = true;
                gridCollisionFlags.Cright = hits[i].collider;
                break;

            }
        }

        

    }

    private void setCollisionCount()
    {
        collisionCount = 0;
        if (gridCollisionFlags.Cabove!=null&&(gridCollisionFlags.Cabove.tag == "Player" || gridCollisionFlags.Cabove.tag == "Block"))
        {
            ++collisionCount;
        }
        if (gridCollisionFlags.Cbelow!=null&&(gridCollisionFlags.Cbelow.tag == "Player" || gridCollisionFlags.Cbelow.tag == "Block"))
        {
            ++collisionCount;
        }
        if (gridCollisionFlags.Cleft!=null&&(gridCollisionFlags.Cleft.tag == "Player" || gridCollisionFlags.Cleft.tag == "Block"))
        {
            ++collisionCount;
        }
        if (gridCollisionFlags.Cright!=null&&(gridCollisionFlags.Cright.tag == "Player" || gridCollisionFlags.Cright.tag == "Block"))
        {
            ++collisionCount;
        }
    }

    

    void CalculateRayOrigins(BoxCollider2D col = null){
        if(col==null){
            col = collider;
        }

        Bounds bounds = col.bounds;
        bounds.Expand(-1 * skinWidth);

        rayCastOrigins.up = new Vector2(bounds.min.x+bounds.size.x/2,bounds.max.y);
        rayCastOrigins.down = new Vector2(bounds.min.x+bounds.size.x/2,bounds.min.y);
        rayCastOrigins.left = new Vector2(bounds.min.x,bounds.min.y+bounds.size.y/2);
        rayCastOrigins.right = new Vector2(bounds.max.x,bounds.min.y+bounds.size.y/2);


    }
}

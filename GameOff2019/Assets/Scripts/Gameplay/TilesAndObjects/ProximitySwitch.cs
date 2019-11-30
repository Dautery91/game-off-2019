using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// this is the base class of switch. It activates all objects in the level of same color within the radius of jumpCount.
public class ProximitySwitch : MonoBehaviour
{
    public ObjectColor objectColor;

    List<IObject> LinkedObjects;

    [SerializeField] IntData jumpCount;

    List<IObject> ActivatedPersistentObjects;

    Vector3Int cellPos;
    Tilemap tilemap;
    

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        AcquireLinkedObjects();
        tilemap = GetComponentInParent<Tilemap>();
        if(tilemap == null){
            cellPos = new Vector3Int(0,0,0);
            
            return;
        }
      
        cellPos = tilemap.WorldToCell(transform.position);
    }

    void AcquireLinkedObjects(){

        LinkedObjects = new List<IObject>();
        ActivatedPersistentObjects = new List<IObject>();
        IObject[] iobjects = (IObject[])GameObject.FindObjectsOfType(typeof(IObject));

        foreach (var iobject in iobjects)
        {
            if(iobject.objectColor == objectColor){
                LinkedObjects.Add(iobject);
            }
        }
    }


    protected virtual void playerEnteredSwitch(){
        foreach(IObject iobject in LinkedObjects){
            
            int radius = jumpCount.Data;
            if(Mathf.Abs(cellPos.x-iobject.cellPos.x)<=radius&&Mathf.Abs(cellPos.y-iobject.cellPos.y)<=radius){
                iobject.ToggleState();
                if(iobject.persistent){
                    if(!ActivatedPersistentObjects.Contains(iobject)){
                        ActivatedPersistentObjects.Add(iobject);
                    }
                }
            }

            
        }
    }

    protected virtual void playerLeftSwitch(){
        foreach(IObject iobject in ActivatedPersistentObjects){
            iobject.ToggleState();
        }
        ActivatedPersistentObjects = new List<IObject>();
    }


    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player"){
            playerEnteredSwitch();
        }
        
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player"){
            playerLeftSwitch();
        }
        
    }
}

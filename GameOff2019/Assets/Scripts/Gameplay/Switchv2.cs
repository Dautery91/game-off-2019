using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// this is the base class of switch. It activates all objects in the level of same color within the radius of jumpCount.
public class Switchv2 : MonoBehaviour
{
    public ObjectColor objectColor;

    List<IObject> LinkedObjects;

    [SerializeField] IntData jumpCount;

    [SerializeField] GameObject EffectSprite;

    List<IObject> ActivatedPersistentObjects;
    List<GameObject> EffectIndicatorSprites;

    Vector3Int cellPos;
    Tilemap tilemap;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        EffectIndicatorSprites = new List<GameObject>();

        AcquireLinkedObjects();
        tilemap = GetComponentInParent<Tilemap>();
        if (tilemap == null)
        {
            cellPos = new Vector3Int(0, 0, 0);
            Debug.LogError("No tilemap in parent");
            return;
        }

        cellPos = tilemap.WorldToCell(transform.position);
    }

    void AcquireLinkedObjects()
    {

        LinkedObjects = new List<IObject>();
        ActivatedPersistentObjects = new List<IObject>();
        IObject[] iobjects = (IObject[])GameObject.FindObjectsOfType(typeof(IObject));

        foreach (var iobject in iobjects)
        {
            if (iobject.objectColor == objectColor)
            {
                LinkedObjects.Add(iobject);
            }
        }
    }


    protected virtual void playerEnteredSwitch()
    {
        int radius = jumpCount.Data;

        // Probably should use pooling
        for (int i = 1; i < radius + 1; i++)
        {
            GameObject effectBlockNegativeX = Instantiate(EffectSprite, new Vector2(cellPos.x - i, cellPos.y), Quaternion.identity);
            GameObject effectBlockPositiveX = Instantiate(EffectSprite, new Vector2(cellPos.x + i, cellPos.y), Quaternion.identity);
            GameObject effectBlockNegativeY = Instantiate(EffectSprite, new Vector2(cellPos.x, cellPos.y - i), Quaternion.identity);
            GameObject effectBlockPositiveY = Instantiate(EffectSprite, new Vector2(cellPos.x, cellPos.y + i), Quaternion.identity);

            EffectIndicatorSprites.Add(effectBlockNegativeX);
            EffectIndicatorSprites.Add(effectBlockPositiveX);
            EffectIndicatorSprites.Add(effectBlockNegativeY);
            EffectIndicatorSprites.Add(effectBlockPositiveY);

        }

        foreach (IObject iobject in LinkedObjects)
        {

            
            if (Mathf.Abs(cellPos.x - iobject.cellPos.x) <= radius && Mathf.Abs(cellPos.y - iobject.cellPos.y) <= radius)
            {
                iobject.ToggleState();
                if (iobject.persistent)
                {
                    if (!ActivatedPersistentObjects.Contains(iobject))
                    {
                        ActivatedPersistentObjects.Add(iobject);
                    }
                }
            }


        }
    }

    protected virtual void playerLeftSwitch()
    {
        foreach (GameObject go in EffectIndicatorSprites)
        {
            Destroy(go);
            
        }

        EffectIndicatorSprites.Clear();

        foreach (IObject iobject in ActivatedPersistentObjects)
        {
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
        if (other.tag == "Player")
        {
            playerEnteredSwitch();
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerLeftSwitch();
        }

    }
}

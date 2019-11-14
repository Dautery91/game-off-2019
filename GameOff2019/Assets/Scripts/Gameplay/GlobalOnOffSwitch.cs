using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlobalOnOffSwitch : IObject
{
    //public ObjectColor objectColor;

    List<IObject> LinkedObjects;

    List<IObject> ActivatedPersistentObjects;
    List<GameObject> EffectIndicatorSprites;

    Tilemap tilemap;

    [SerializeField] GameObject SwitchUnpressedSprite;
    [SerializeField] GameObject SwitchPressedSprite;


    void Start()
    {
        AcquireLinkedObjects();
        tilemap = GetComponentInParent<Tilemap>();
        if (tilemap == null)
        {
            cellPos = new Vector3Int(0, 0, 0);
            Debug.LogError("No tilemap in parent");
            return;
        }
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
        this.ToggleState();

        foreach (IObject iobject in LinkedObjects)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerEnteredSwitch();
        }

    }

    public override void TurnOn()
    {
        SwitchPressedSprite.SetActive(true);
        SwitchUnpressedSprite.SetActive(false);
    }

    public override void TurnOff()
    {
        SwitchPressedSprite.SetActive(false);
        SwitchUnpressedSprite.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GlobalOnOffSwitch : IObject
{
    List<IObject> LinkedObjects;

    List<IObject> ActivatedPersistentObjects;
    List<GameObject> EffectIndicatorSprites;

    Tilemap tilemap;

    [SerializeField] GameObject SwitchUnpressedSprite;
    [SerializeField] GameObject SwitchPressedSprite;


    [SerializeField] VoidGameEvent JumpInterruptEvent;

    Timer eventCooldownTimer;

    void Start()
    {
        eventCooldownTimer = this.gameObject.AddComponent<Timer>();
        eventCooldownTimer.Duration = 0.15f;
        eventCooldownTimer.Run();

        tilemap = GetComponentInParent<Tilemap>();

        if (tilemap == null)
        {
            //cellPos = new Vector3Int(0, 0, 0);
            Debug.LogError("No tilemap in parent");
            return;
        }

        AcquireLinkedObjects();
    }


    void AcquireLinkedObjects()
    {
        LinkedObjects = new List<IObject>();
        ActivatedPersistentObjects = new List<IObject>();
        IObject[] iobjects = (IObject[])GameObject.FindObjectsOfType(typeof(IObject));

        foreach (var iobject in iobjects)
        {
            if (iobject.objectColor == objectColor && iobject.gameObject != this.gameObject)
            {
                LinkedObjects.Add(iobject);
            }
        }
    }

    protected virtual void SwitchEntered()
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

        // Triggers player to recalculate their jump distance in case new blocks obstruct their path
        if (eventCooldownTimer.Finished && !eventCooldownTimer.Running)
        {
            JumpInterruptEvent.Raise();
            eventCooldownTimer.Run();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Block")
        {
            SwitchEntered();
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

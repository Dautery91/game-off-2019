using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffBlockTile : IObject
{

    public GameObject OnSprite;
    public GameObject OffSprite;

    public override void TurnOn()
    {
        OnSprite.SetActive(true);
        OffSprite.SetActive(false);
    }

    public override void TurnOff()
    {
        OnSprite.SetActive(false);
        OffSprite.SetActive(true);
    }

}

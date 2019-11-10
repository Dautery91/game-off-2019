using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class IntData : ScriptableObject
{
    protected int data;

    //override this in child classes as required
    public int Data{
        get{
            return data;
        }
        set{
           data = value;
        }
    }
}

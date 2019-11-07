using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntData : MonoBehaviour
{
    protected int data;

    //override this in child classes as required
    public int Data{
        get{
            return data;
        }
        set{
            value = data;
        }
    }
}

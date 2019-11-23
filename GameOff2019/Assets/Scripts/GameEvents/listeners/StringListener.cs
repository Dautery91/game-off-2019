using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StringListener : BaseGameEventListener<string,StringGameEvent,UnityStringEvent>{
    //public override void OnEventRaised(string item)
    //{
    //    UnityEventResponse.Invoke(item);
    //    //base.OnEventRaised(item);
    //    Debug.LogError("String event raised: " + item);
    //}
}

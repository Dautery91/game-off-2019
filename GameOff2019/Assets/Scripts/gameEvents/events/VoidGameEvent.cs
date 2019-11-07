using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    
[CreateAssetMenu(fileName="new void event",menuName="Game events/Void event")]
public class VoidGameEvent : BaseGameEvent<Void>{

    public void Raise(){
        Raise(new Void());
    }
} 

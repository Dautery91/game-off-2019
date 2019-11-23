using UnityEngine;
using System.Collections.Generic;
using System;


public abstract class BaseGameEvent<T> : ScriptableObject{

    private List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

    public virtual void Raise(T item){
        for (int i = listeners.Count - 1; i >= 0 ; i--)
        {
            listeners[i].OnEventRaised(item);
        }
    }

    public virtual void RegisterListener(IGameEventListener<T> listener){
        if(!listeners.Contains(listener)){
            listeners.Add(listener);
        }
    }

    public virtual void UnregisterListener(IGameEventListener<T> listener){
        if(listeners.Contains(listener)){
            listeners.Remove(listener);
        }
    }
}



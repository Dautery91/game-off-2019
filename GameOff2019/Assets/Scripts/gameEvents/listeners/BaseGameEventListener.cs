
using UnityEngine;
using UnityEngine.Events;


public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour,
IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
{

    public UER UnityEventResponse;
    public E gameEvent;

    private void Start()
    {
        if (isActiveAndEnabled)
        {
            if (gameEvent == null) { return; }

            gameEvent.RegisterListener(this);
        }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if(gameEvent == null){ return; }

        gameEvent.RegisterListener(this);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        
        if(gameEvent == null){ return; }

        gameEvent.UnregisterListener(this);
    }

    public virtual void OnEventRaised(T item){
        if(UnityEventResponse!=null){
            UnityEventResponse.Invoke(item);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public struct EventParam
{
    public string stringParam;
    public int intParam;
    public float floatParam;
    public bool boolParam;
}

public static class EventManager
{
    static Dictionary<EventNames, Action<EventParam>> eventDictionary = new Dictionary<EventNames, Action<EventParam>>();

    public static void AddListener(EventNames eventName, Action<EventParam> listenerMethod)
    {
        Action<EventParam> thisEvent;

        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listenerMethod;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listenerMethod;
            eventDictionary.Add(eventName, listenerMethod);
        }
    }

    public static void StopListening(EventNames eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;

            eventDictionary[eventName] = thisEvent;
        }
    }

    public static void RaiseEvent(EventNames eventName, EventParam eventParam)
    {
        Action<EventParam> thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(eventParam);
        }
    }
}

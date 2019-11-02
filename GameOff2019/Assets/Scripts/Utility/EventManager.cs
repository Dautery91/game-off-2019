using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// An event manager.  Only works with delegates who dont require input parameters
/// Can make more general later
/// </summary>
public static class EventManager
{
    static Dictionary<EventNames, List<Action>> eventDictionary = new Dictionary<EventNames, List<Action>>();

    public static void AddListener(EventNames eventName, Action listenerMethod)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName].Add(listenerMethod);
        }
        else
        {
            eventDictionary.Add(eventName, new List<Action>());
            eventDictionary[eventName].Add(listenerMethod);
        }
    }

    public static void RemoveListener(EventNames eventName, Action listenerMethod)
    {
        if (eventDictionary[eventName].Contains(listenerMethod))
        {
            eventDictionary[eventName].Remove(listenerMethod);
        }
        else
        {
            Debug.Log("There is no method: " + listenerMethod +
                " registered to the following event: " + eventName);
        }
    }

    public static void RaiseEvent(EventNames eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            foreach (var listenerMethod in eventDictionary[eventName])
            {
                listenerMethod.Invoke();
            }
        }
    }
}

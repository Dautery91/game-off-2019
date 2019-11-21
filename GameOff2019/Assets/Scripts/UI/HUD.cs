using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Text hudText;

    [SerializeField] IntData JumpCount;
    const string HUDTextPrefix = "Jump Height: ";
    //private Action<EventParam> jumpEventListener;

    private void Start()
    {
        // Need to figure out initialization
        hudText.text = HUDTextPrefix + 0;
        // jumpEventListener = new Action<EventParam>(HandleJumpEvent);
        // EventManager.AddListener(EventNames.JumpUpdateEvent, jumpEventListener);
    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(JumpCount!=null){
            HandleJumpEvent(JumpCount.Data);

        }
    }



    public void HandleJumpEvent(int jumpStrength)
    {
        hudText.text = HUDTextPrefix + jumpStrength;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Text JumpCountText;

    [SerializeField] IntData JumpCount;
    const string HUDTextPrefix = "Leap Strength: ";

    [SerializeField] Text TimerText;
    const string timerPrefix = "Time Elapsed: ";
    StopWatch gameTimer;
    

    private void Awake()
    {
        gameTimer = FindObjectOfType<StopWatch>();
    }

    private void Start()
    {
        JumpCountText.text = HUDTextPrefix + 0;
        gameTimer.StartTiming();
        TimerText.text = timerPrefix;

    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(JumpCount!=null){
            HandleJumpEvent(JumpCount.Data);

        }

        if (!gameTimer.IsDisabled)
        {
            TimerText.text = timerPrefix + gameTimer.ElapsedSeconds.ToString("0.00");
        }
        else
        {
            TimerText.text = timerPrefix + "Disabled!";
        }
        
    }



    public void HandleJumpEvent(int jumpStrength)
    {
        JumpCountText.text = HUDTextPrefix + jumpStrength;
    }

    public void DisableGameTimer()
    {
        TimerText.text = timerPrefix + "Disabled!";
        gameTimer.DisableTimer();
    }
}

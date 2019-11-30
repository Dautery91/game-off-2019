using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : MonoBehaviour
{
    #region Fields

    // timer execution
    float elapsedSeconds = 0;
    bool running = false;

    // support for TimerFinished event
    bool started = false;

    bool timerDisabled = false;

    #endregion

    public float ElapsedSeconds
    {
        get { return elapsedSeconds; }
        set { elapsedSeconds = value; }
    }

    public bool IsRunning
    {
        get { return running; }
    }

    public bool IsDisabled
    {
        get { return timerDisabled; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (running  && !timerDisabled)
        {
            elapsedSeconds += Time.deltaTime;
           
        }
    }


    public void StartTiming()
    {
         running = true;
         started = true;
    }

    public void PauseTimer()
    {
        running = false;
    }

    public void ResetTimer()
    {
        
        running = false;
        started = false;
        elapsedSeconds = 0;
    }

    public void DisableTimer()
    {
        timerDisabled = true;
    }

    public void EnableTimer()
    {
        timerDisabled = false;

    }
}

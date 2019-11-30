using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalLevelCanvas : MonoBehaviour
{
    [SerializeField] Text FinalTimerText;
    StopWatch gameTimer;

    private void Start()
    {
        gameTimer = FindObjectOfType<StopWatch>();

        if (!gameTimer.IsDisabled)
        {
            FinalTimerText.text = "Final Time: " + FindObjectOfType<StopWatch>().ElapsedSeconds.ToString("0.00") + " Seconds";
        }
        else
        {
            FinalTimerText.text = "Final Time:  Disabled";
        }
        
        
    }

}

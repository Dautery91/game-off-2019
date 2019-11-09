using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] VoidGameEvent GameResumeEvent;
    [SerializeField] VoidGameEvent RespawnPlayerEvent;

    public void ResumeGame(){
        GameResumeEvent.Raise();
    }

    public void Retry(){
        GameResumeEvent.Raise();
        RespawnPlayerEvent.Raise();
    }

    public void MainMenu(){
        GameManager.instance.LoadMainMenu();
    }
}

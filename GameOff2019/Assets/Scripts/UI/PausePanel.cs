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
        GameManager.instance.ResetLevel();
    }

    public void MainMenu(){
        GameManager.instance.LoadMainMenu();
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}

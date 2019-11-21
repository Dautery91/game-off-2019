using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] VoidGameEvent GameResumeEvent;
    [SerializeField] VoidGameEvent RespawnPlayerEvent;

    public void ResumeGame(){
        AudioManager.instance.PlayButtonClickSound();
        GameResumeEvent.Raise();
    }

    public void Retry(){
        AudioManager.instance.PlayButtonClickSound();
        GameManager.instance.ResetLevel();
    }

    public void MainMenu(){
        AudioManager.instance.PlayButtonClickSound();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameOverPanel : MonoBehaviour
{
    [SerializeField] VoidGameEvent RespawnPlayerEvent;
    [SerializeField] VoidGameEvent GameResumeEvent;

    public GameObject NextButton;

    public Text ResultText;


    public void Setup(bool levelCleared, string reason){
        if(levelCleared){
            ResultText.text = reason;
            NextButton.SetActive(true);
        }
        else{
            ResultText.text = reason;
            NextButton.SetActive(false);
        }
    }

    public void NextLevel(){
        AudioManager.instance.PlayButtonClickSound();
        GameManager.instance.LoadNextLevel();
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

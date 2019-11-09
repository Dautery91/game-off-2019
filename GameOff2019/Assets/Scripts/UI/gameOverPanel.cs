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


    public void Setup(bool levelCleared){
        if(levelCleared){
            ResultText.text = "Level Cleared!";
            NextButton.SetActive(true);
        }
        else{
            ResultText.text = "Level Failed!";
            NextButton.SetActive(false);
        }
    }

    public void NextLevel(){
        GameManager.instance.LoadNextLevel();
    }

    public void Retry(){
        RespawnPlayerEvent.Raise();
        GameResumeEvent.Raise();
    }

    public void MainMenu(){
        GameManager.instance.LoadMainMenu();
    }

    

}

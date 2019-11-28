using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;
    public GameObject LevelSelectPanel;

    public void OnGamePaused(){
        if (!pauseMenuPanel.activeSelf && !gameOverPanel.activeSelf)
        {
            pauseMenuPanel.SetActive(true);
        }
        else
        {
            pauseMenuPanel.SetActive(false);
            LevelSelectPanel.SetActive(false);
        }
    }

    public void OnGameResume(){

        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        LevelSelectPanel.SetActive(false);
    }

    public void OnLevelCleared(){
        //GameManager.instance.PauseGame();
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<gameOverPanel>().Setup(true, "Level Cleared!");

    }

    public void OnLevelFailed(string stringReason){

        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<gameOverPanel>().Setup(false, stringReason);
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;

    public void OnGamePaused(){
        pauseMenuPanel.SetActive(true);
    }

    public void OnGameResume(){
        pauseMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void OnLevelCleared(){
        GameManager.instance.PauseGame();
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<gameOverPanel>().Setup(true);

    }

    public void OnLevelFailed(){

        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponent<gameOverPanel>().Setup(false);
        
    }
}

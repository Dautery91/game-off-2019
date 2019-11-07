using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void HandlePlayButtonOnClickEvent()
    {
        SceneManager.LoadScene("Level1");
    }

    /// <summary>
    /// Handles the on click event from the high score button
    /// </summary>
    public void HandleOptionsButtonOnClickEvent()
    {
        MenuManager.GoToMenu(MenuNamesEnum.OptionsMenu);
    }

    /// <summary>
    /// Handles the on click event from the quit button
    /// </summary>
    public void HandleQuitButtonOnClickEvent()
    {
        Application.Quit();
    }
}

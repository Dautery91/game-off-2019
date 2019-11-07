using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages navigation through the menu system
/// </summary>
public static class MenuManager
{
    /// <summary>
    /// PLACEHOLDER: Goes to the menu with the given name
    /// </summary>
    /// <param name="name">name of the menu to go to</param>
    public static void GoToMenu(MenuNamesEnum name)
    {
        switch (name)
        {
            case MenuNamesEnum.MainMenu:

                // go to MainMenu scene
                SceneManager.LoadScene("MainMenu");
                break;
            case MenuNamesEnum.OptionsMenu:

                // instantiate prefab
                Object.Instantiate(Resources.Load("OptionsMenu"));
                break;
            case MenuNamesEnum.PauseMenu:

                // instantiate prefab
                Object.Instantiate(Resources.Load("PauseMenu"));
                break;
        }
    }
}


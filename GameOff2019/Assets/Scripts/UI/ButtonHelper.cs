using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public void PlayButtonClickSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonClickSound();
        }

    }

    public void PlayButtonClickBackSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayButtonClickBackSound();
        }
        
    }

    public void LinkToTwitter()
    {
        Application.OpenURL("https://twitter.com/DrakesGames");
    }

}

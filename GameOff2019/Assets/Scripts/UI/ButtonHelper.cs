using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHelper : MonoBehaviour
{
    public void PlayButtonClickSound()
    {
        AudioManager.instance.PlayButtonClickSound();
        
    }

    public void PlayButtonClickBackSound()
    {
        AudioManager.instance.PlayButtonClickBackSound();
    }

    public void LinkToTwitter()
    {
        Application.OpenURL("https://twitter.com/DrakesGames");
    }

}

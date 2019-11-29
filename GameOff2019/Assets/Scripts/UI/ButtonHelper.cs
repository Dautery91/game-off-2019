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

}

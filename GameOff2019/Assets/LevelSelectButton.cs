using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public level level;
    public Text buttonText;

    public void Initialize(){
        buttonText.text = level.name;
    }

    public void levelSelect(){
        GameManager.instance.LoadSpecificLevel(level.name);
    }
   
}

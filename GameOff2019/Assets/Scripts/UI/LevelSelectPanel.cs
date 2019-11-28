﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPanel : MonoBehaviour
{
    [SerializeField] Text InputText; 
    private string levelName;

    public void LoadInputLevel()
    {
        
        levelName = InputText.text;
        GameManager.instance.LoadSpecificLevel(levelName);
    }
}

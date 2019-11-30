using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectPanel : MonoBehaviour
{
    [SerializeField] Text InputText; 
    private string levelName;

    [SerializeField] GameObject levelContentGroup;

    [SerializeField] GameObject levelPrefab;
    [SerializeField] Levels levels;


    public void LoadInputLevel()
    {
        
        levelName = InputText.text;
        GameManager.instance.LoadSpecificLevel(levelName);
    }

    private void InitializeLevelsList(){
        for(int i=0; i < levels.levels.Length;i++){
            level l = levels.levels[i];
            GameObject levelObject = Instantiate(levelPrefab,levelContentGroup.transform);
            LevelSelectButton levelSelectButton = levelObject.GetComponent<LevelSelectButton>();
            levelSelectButton.level = l;
            levelSelectButton.Initialize();
        }
        
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        InitializeLevelsList();
    }
}

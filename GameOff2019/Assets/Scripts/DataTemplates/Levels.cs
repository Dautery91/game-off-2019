using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="new Levels object",menuName="data objects/levels object")]
public class Levels : ScriptableObject
{
    [SerializeField]
    public level[] levels;

    public level startLevel;

    [HideInInspector] public level currentLevel;

    public level getNextLevel(string lname){
        for (int i = 0; i < levels.Length; i++)
        {
            if(levels[i].name == lname&&i+1<levels.Length){
                return levels[i+1];
            }
        }
        return null;
    }
    
    public level getPrevLevel(string lname){
        for (int i = 0; i < levels.Length; i++)
        {
            if(levels[i].name == lname&&i-1>=0){
                return levels[i-1];
            }
        }
        return null;
    }

}

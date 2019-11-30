using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] VoidGameEvent TouchScreenToggleEvent;
    private bool isTouchScreenMode = false;

    [SerializeField] Levels LevelData;
    [SerializeField] VoidGameEvent GamePauseEvent;

    private StopWatch gameTimer;
    
    public float GameTimeElapsed
    {
        get
        {
            return gameTimer.ElapsedSeconds;
        }
        set
        {
            gameTimer.ElapsedSeconds = value;
        }
    }

    //singleton stuff
    public static GameManager instance;
    private bool gamePaused = false;

    public bool IsTouchScreenMode { get { return isTouchScreenMode; } set { isTouchScreenMode = value; } }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {

        if (GameManager.instance != null && GameManager.instance!= this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            
        }
    }

    public void OnResume(){
        gamePaused = false;
    }


    public void ResetLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(LevelData.GetFirstLevel().name);
        FindObjectOfType<StopWatch>().StartTiming();
    }

    public void LoadNextLevel(){
        level nextLevel = LevelData.getNextLevel(SceneManager.GetActiveScene().name);
        if(nextLevel!=null){
            LevelData.currentLevel = nextLevel;
            SceneManager.LoadScene(nextLevel.name);
        }
        else{
            
        }
    }
    public void LoadPrevLevel(){
        level PrevLevel = LevelData.getPrevLevel(SceneManager.GetActiveScene().name);
        if(PrevLevel!=null){
            SceneManager.LoadScene(PrevLevel.name);
        }
        else{
           
        }
    }

    public void LoadMainMenu(){
        level MainMenuLevel = LevelData.startLevel;
        if(MainMenuLevel!=null){
            SceneManager.LoadScene(MainMenuLevel.name);
            FindObjectOfType<StopWatch>().ResetTimer();
        }
        else{
            
        }
    }

    public void LoadSpecificLevel(string levelName)
    {
        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            
        }
    }

    private void InitializeGameManager()
    {
        GameManager.instance.IsTouchScreenMode = this.isTouchScreenMode;
        if (isTouchScreenMode)
        {
            GameManager.instance.ToggleTouchScreenMode();
        }
    }

    public void ToggleTouchScreenMode()
    {
        isTouchScreenMode = !isTouchScreenMode;
        TouchScreenToggleEvent.Raise();
        
    }

    private void OnLevelWasLoaded(int level)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopSound("ElecOn");
        }
        
    }

    public void StartGameTimer()
    {
        gameTimer.StartTiming();
    }

    public void ResetGameTimer()
    {
        gameTimer.ResetTimer();
    }

}

using UnityEngine;

/// <summary>
/// Script for reading input.  Wanted to keep it distinct from movement so we can mess with the inputs / buttons seperately from the movement.
/// </summary>
public class PlayerInputReader : MonoBehaviour
{
    private float horizontalMoveInput;
    private bool playerRetryInput;
    private bool jumpInput;

    private bool gamePaused = false;

    public bool GamePaused{
        get{
            return gamePaused;
        }
    }
    private float rotateInput;

    [SerializeField] VoidGameEvent GamePauseEvent;

    public float HorizontalMoveInput { get { return horizontalMoveInput; } }
    public bool PlayerRetryInput { get { return playerRetryInput; } }
    public bool JumpInput { get { return jumpInput; } }
    public float RotateInput { get { return rotateInput; } }



    void Update()
    {
       
        ReadMovementInput();
        ReadJumpInput();
        ReadPauseInput();
        
        
    }

    private void ReadMovementInput()
    {
        if(!gamePaused){
            horizontalMoveInput = Input.GetAxisRaw("Horizontal");
        }
    }


    private void ReadJumpInput()
    {   //@DAUTERY I'm changing the key to W as space bar kept triggering my on resume for some reason
        if (Input.GetKeyDown(KeyCode.W) && !gamePaused)
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }
    }

    private void ReadPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
        {
            gamePaused = true;
            GamePauseEvent.Raise();
        }
        
    }

    public void onPlayerDeath(){
        gamePaused = true;
        GamePauseEvent.Raise();
    }

    public void OnResume(){
        gamePaused = false;
    }

}

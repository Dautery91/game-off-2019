using UnityEngine;

/// <summary>
/// Script for reading input.  Wanted to keep it distinct from movement so we can mess with the inputs / buttons seperately from the movement.
/// </summary>
public class PlayerInputReader : MonoBehaviour
{
    private float horizontalMoveInput = 0f;
    private bool playerRetryInput;
    private bool jumpInput;
    private bool normalJumpInput;
    private bool gamePaused = false;

    public bool GamePaused{
        get{
            return gamePaused;
        }
    }
    private float rotateInput;
    public float HorizontalMoveInput { get { return horizontalMoveInput; } }
    public bool PlayerRetryInput { get { return playerRetryInput; } }
    public bool JumpInput { get { return jumpInput; } }
    public bool NormalJumpInput { get { return normalJumpInput; } }
    public float RotateInput { get { return rotateInput; } }



    void Update()
    {
       
        ReadMovementInput();
        ReadJumpInput();
        
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
        if (Input.GetKeyDown(KeyCode.Q) && !gamePaused)
        {
            normalJumpInput = true;
        }
        else
        {
            normalJumpInput = false;
        }
    }


    public void onPause(){
        gamePaused = true;
    }

    public void OnResume(){
        gamePaused = false;
    }

}

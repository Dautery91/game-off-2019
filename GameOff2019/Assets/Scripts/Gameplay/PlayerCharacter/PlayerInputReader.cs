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

    [SerializeField] VoidGameEvent GamePauseEvent;


    public float HorizontalMoveInput { get { return horizontalMoveInput; } }
    public bool PlayerRetryInput { get { return playerRetryInput; } }
    public bool JumpInput { get { return jumpInput; } }



    void Update()
    {
       
        ReadMovementInput();
        ReadJumpInput();
        ReadPauseInput();
        
    }

    private void ReadMovementInput()
    {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");

    }


    private void ReadJumpInput()
    {   
        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.W)))
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePauseEvent.Raise();
        }

    }

}

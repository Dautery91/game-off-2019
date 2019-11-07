using UnityEngine;

/// <summary>
/// Script for reading input.  Wanted to keep it distinct from movement so we can mess with the inputs / buttons seperately from the movement.
/// </summary>
public class PlayerInputReader : MonoBehaviour
{
    private float horizontalMoveInput;
    private bool playerRetryInput;
    private bool jumpInput;
    private float rotateInput;

    public float HorizontalMoveInput { get { return horizontalMoveInput; } }
    public bool PlayerRetryInput { get { return playerRetryInput; } }
    public bool JumpInput { get { return jumpInput; } }
    public float RotateInput { get { return rotateInput; } }



    void Update()
    {
        ReadMovementInput();
        ReadJumpInput();
        ReadRetryInput();
    }

    private void ReadMovementInput()
    {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");
    }


    private void ReadJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
        }
        else
        {
            jumpInput = false;
        }
    }

    private void ReadRetryInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playerRetryInput = true;
        }
        else
        {
            playerRetryInput = false;
        }
    }

}

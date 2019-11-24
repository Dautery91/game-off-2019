using UnityEngine;

/// <summary>
/// Script for reading input.  Wanted to keep it distinct from movement so we can mess with the inputs / buttons seperately from the movement.
/// </summary>
public class PlayerInputReaderAndroid : MonoBehaviour
{
    private float horizontalMoveInput = 0f;
    private bool playerRetryInput;
    private bool jumpInput;
    private bool normalJumpInput;
    private Touch touch;

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

        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);

            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touchPosition.x >= this.transform.position.x)
            {
                horizontalMoveInput = 1;
            }
            else if (touchPosition.x < this.transform.position.x)
            {
                horizontalMoveInput = -1;
            }
            else
            {
                horizontalMoveInput = 0;
            }

        }
        else
        {
            horizontalMoveInput = 0;
        }

    }


    private void ReadJumpInput()
    {
        if (Input.touchCount == 2)
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
        if (Input.touchCount == 3)
        {
            GamePauseEvent.Raise();
        }

    }

}

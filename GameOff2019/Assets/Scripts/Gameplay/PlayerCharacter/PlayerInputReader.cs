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

    private bool startedHoldingReset = false;
    private Timer resetTimer;
    private const float ResetHoldTime = 1;

    private bool isTouchScreenMode = false;

    [SerializeField] VoidGameEvent GamePauseEvent;


    public float HorizontalMoveInput { get { return horizontalMoveInput; } }
    public bool PlayerRetryInput { get { return playerRetryInput; } }
    public bool JumpInput { get { return jumpInput; } }

    private void Awake()
    {
        resetTimer = this.gameObject.AddComponent<Timer>();
        resetTimer.Duration = ResetHoldTime;
    }

    void Update()
    {
        ReadJumpInput();
        ReadMovementInput();
        ReadPauseInput();
        ReadResetInput();

        //if (isTouchScreenMode)
        //{
        //    Debug.Log("in touch screen mode");
        //}

    }

    private void ReadMovementInput()
    {
        horizontalMoveInput = Input.GetAxisRaw("Horizontal");
    }


    private void ReadJumpInput()
    {   
        if ((Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)))
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

    private void ReadResetInput()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            startedHoldingReset = true;
            resetTimer.Run();
        }

        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            startedHoldingReset = false;
            resetTimer.Stop();
        }

        if (resetTimer.Finished)
        {
            resetTimer.Stop();
            startedHoldingReset = false;
            GameManager.instance.ResetLevel();
        }
    }

    public void ToggleTouchScreenMode()
    {
        isTouchScreenMode = GameManager.instance.IsTouchScreenMode;
    }



}

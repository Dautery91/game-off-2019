using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterEngine : MonoBehaviour
{
    // Scriptable object data injection
    [SerializeField] CharacterData CharacterData;

    // Movement support
    private Rigidbody2D rb2D;
    private PlayerInputReader characterInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    private const float groundCheckRadius = 0.1f;

    // Jump support
    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask GroundLayer;
    private int jumpStoredAmount;
    private float jumpPositionY;
    private float landPositionY;
    private float timeToJumpDestination;


    private void Awake()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        characterInput = this.GetComponent<PlayerInputReader>();
        jumpStoredAmount = 5;
    }

    private void FixedUpdate()
    {
        MoveCharacterHorizontal();
        Jump();
    }


    #region Horizontal Movement
    private void MoveCharacterHorizontal()
    {
        rb2D.velocity = new Vector2(characterInput.HorizontalMoveInput * CharacterData.MoveSpeed, rb2D.velocity.y);

        if (characterInput.HorizontalMoveInput > 0 && isFacingRight == false)
        {
            FlipFacingDirection();
        }
        else if (characterInput.HorizontalMoveInput < 0 && isFacingRight == true)
        {
            FlipFacingDirection();
        }

    }

    private void FlipFacingDirection()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    #endregion

    private void Jump()
    {
        timeToJumpDestination += Time.fixedDeltaTime / CharacterData.InverseJumpSpeed;

        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, GroundLayer);

        if (characterInput.JumpInput && isGrounded)
        {
            float startPositionY = rb2D.position.y;

            //transform.position = Vector2.Lerp(new Vector2(rb2D.position.x, startPositionY), new Vector2(rb2D.position.x, startPositionY + jumpPositionY), timeToJumpDestination);

            rb2D.MovePosition(new Vector2(rb2D.position.x, rb2D.position.y + jumpStoredAmount));
            jumpStoredAmount = 0;
            jumpPositionY = rb2D.position.y;

            // might have to delete this later
            isGrounded = false;
        }

    }


}

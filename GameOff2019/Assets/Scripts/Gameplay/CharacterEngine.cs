using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterEngine : MonoBehaviour
{
    // Movement support
    private Rigidbody2D rb2D;
    private PlayerInputReader characterInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    private const float groundCheckRadius = 0.1f;

    [SerializeField]
    CharacterData CharacterData;

    public Transform GroundCheck;
    public LayerMask GroundLayer;

    // Pogo ability support (to pull out in to seperate script)
    private bool isAbilityActive = false;
    private PogoAbility pogoScript;

    private void Awake()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        characterInput = this.GetComponent<PlayerInputReader>();

    }

    private void Start()
    {
        EventManager.AddListener(EventNames.AbilityActivatedEvent, HandleAbilityActivatedEvent);
    }

    // Note:  I could also see making jump an event as opposed to polling for it in the fixedupdate method
    private void FixedUpdate()
    {
        MoveCharacter();

        InitialJump();
    }


    #region Horizontal Movement
    private void MoveCharacter()
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

    private void InitialJump()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, GroundLayer);

        if (characterInput.JumpInput && isGrounded && !isAbilityActive)
        {
            rb2D.velocity = Vector2.up * CharacterData.JumpForce;

            // First jump activates pogo or skateboard
            EventManager.RaiseEvent(EventNames.AbilityActivatedEvent);
        }

    }

    // Ideally want to change this to take an ability type parameter
    public void HandleAbilityActivatedEvent()
    {
        isAbilityActive = true;
        //EventManager.RemoveListener(EventNames.AbilityActivatedEvent, HandleAbilityActivatedEvent);
    }


}

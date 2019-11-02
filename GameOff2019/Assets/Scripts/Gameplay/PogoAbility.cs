using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PogoAbility : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private PlayerInputReader characterInput;
    private Vector2 bounceDirection;
    private bool isActive = false;
    private bool slammed = false;
    private float rotationAmount;

    public PogoData pogoData;


    private void Awake()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
        characterInput = this.GetComponent<PlayerInputReader>();
    }

    private void Start()
    {
        EventManager.AddListener(EventNames.AbilityActivatedEvent, HandlePogoActivatedEvent);
    }

    private void FixedUpdate()
    {
        PogoSlam();
        PogoRotate();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // This is bad.  Should fix so as not to compare tags.  Tags are hard to debug
        if (collision.gameObject.tag == "Ground" && isActive)
        {
            PogoBounce();
        }
    }

    private void PogoBounce()
    {
        // Need to add a directional componenet to this based on rotation
        if (slammed)
        {
            rb2D.AddForce(
                (Vector2.up * pogoData.Bounciness) + 
                new Vector2(0, pogoData.SlamForce / pogoData.SlamForceEnergyLoss),
                ForceMode2D.Impulse);

            slammed = false;
        }
        else
        {
            rb2D.AddForce(Vector2.up * pogoData.Bounciness, ForceMode2D.Impulse);
        }
    }

    // Slam down from above quickly
    private void PogoSlam()
    {
        if (characterInput.JumpInput)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            rb2D.AddForce(Vector2.down * pogoData.SlamForce, ForceMode2D.Impulse);
            slammed = true;
        }
    }

    private void PogoRotate()
    {
        if (characterInput.RotateInput != 0)
        {
            rotationAmount = pogoData.RotationSpeed * Time.fixedDeltaTime;

            if (characterInput.RotateInput < 0)
            {
                rotationAmount *= 1;
                rb2D.freezeRotation = false;
                transform.Rotate(Vector3.forward, rotationAmount);
                rb2D.freezeRotation = true;
            }

            if (characterInput.RotateInput > 0)
            {
                rotationAmount *= -1;
                rb2D.freezeRotation = false;
                transform.Rotate(Vector3.forward, rotationAmount);
                rb2D.freezeRotation = true;
            }
        }
    }

    public void HandlePogoActivatedEvent()
    {
        isActive = true;
        slammed = false;
        //EventManager.RemoveListener(EventNames.AbilityActivatedEvent, HandlePogoActivatedEvent);
    }
}

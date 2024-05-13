using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovementData))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D hitbox;
    [SerializeField] private Transform feetSpot, headSpot;
    [SerializeField] private LayerMask platformLayers;
    [SerializeField] private PlayerMovementData movementData;

    private float directionInput;
    private float remainingJumpBufferTime, remainingJumpCoyoteTime;
    public bool isOnGround, isJumping, isFalling;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
    }


    // Update is called once per frame
    void Update()
    {
        //Movement
        
        directionInput = Input.GetAxisRaw("Horizontal");
        
        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
            remainingJumpBufferTime = movementData.jumpBufferTime;
        // Abort jump TODO move to fixed update when inputs are externalised
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        // Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.position = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
        // Timers
        if(remainingJumpBufferTime > 0f)
        {
            remainingJumpBufferTime -= Time.deltaTime;
        }
        if (remainingJumpCoyoteTime > 0f)
        {
            remainingJumpCoyoteTime -= Time.deltaTime;
        }
        Debug.Log(rb.velocity);
    }

    private void FixedUpdate()
    {
        CheckIsOnGround();
        // Check if we pressed jump recently and were on ground recently
        if (remainingJumpBufferTime > 0f && remainingJumpCoyoteTime > 0f)
        {
            Jump();
        }

        // Change speed
        float desiredSpeed = movementData.baseMaxSpeed * directionInput;
        float acceleration;

        if (desiredSpeed == 0f) // We want to stop
            acceleration = movementData.deceleration;
        else if (isOnGround) // We are running
            acceleration = movementData.baseAcceleration;
        else if (Mathf.Abs(rb.velocity.y) < movementData.apexBoostThreshold)
        { // We are at the top of a jump
            acceleration = movementData.apexBoostAcceleration;
            desiredSpeed = movementData.apexBoostMaxSpeed * directionInput;
        }
        else // We are jumping or falling
            acceleration = movementData.airControl;

        float deltaSpeed = desiredSpeed - rb.velocity.x;
        float movement = deltaSpeed * acceleration;

        rb.AddForce(movement * Vector2.right);
        if(rb.velocity.y < 0f)
        {
            rb.gravityScale = movementData.fallingGravityScale;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -movementData.maxFallingSpeed));
        }
    }

    void CheckIsOnGround()
    {
        Vector3 offset = new Vector3(0.5f, 0f, 0f);
        Debug.DrawRay((feetSpot.position - offset), Vector3.right, Color.red, 0);
        isOnGround = Physics2D.OverlapArea(feetSpot.position - offset, feetSpot.position + offset, platformLayers);
        if (isOnGround)
        {
            remainingJumpCoyoteTime = movementData.jumpCoyoteTime;
            isJumping = false;
            isFalling = false;
            rb.gravityScale = 1f;
        }
    }

    void Jump()
    {
        // If the player is holding the jump button, make a great jump, else make a short jump
        float trueJumpHeight = Input.GetKey(KeyCode.Space) ? movementData.jumpHeight : movementData.miniJumpHeight;
        float jumpForce = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * trueJumpHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        remainingJumpBufferTime = 0f;
        remainingJumpCoyoteTime = 0f;
        isJumping = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D hitbox;
    private SpriteRenderer sprite;
    private Recorder recorder;
    [SerializeField] private Transform feetSpot, headSpot;
    [SerializeField] private LayerMask platformLayers;
    [SerializeField] private PlayerMovementData movementData;


    private float directionInput;
    private float remainingJumpBufferTime, remainingJumpCoyoteTime;
    public bool isOnGround, isJumping;
    private bool isRecording;
    public bool canMove;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        recorder = GetComponent<Recorder>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return;
        }
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
            recorder.CancelCurrentRecording();
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
    }

    public void StartRecording()
    {
        recorder.StartNewRecording();
        isRecording = true;
    }
    public void StopRecording()
    {
        // Stop the record and start the play back
        isRecording = false;
        recorder.StartReplay();
    }

    public void ClearAllRecordings()
    {
        recorder.Clear();
    }

    public void RestartReplay()
    {
        recorder.RestartReplay();
    }

    private void LateUpdate()
    {
        if (isRecording)
        {
            // TODO interact and die this frame
            ReplayData data = new ReplayData(transform.position, rb.velocity, isJumping, sprite.flipX, false, false);
            recorder.RecordReplayData(data);
        }
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
            rb.gravityScale = movementData.gravityScale * movementData.fallingGravityScale;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -movementData.maxFallingSpeed));
        }
    }

    void CheckIsOnGround()
    {
        Vector3 offset = new Vector3(0.5f, 0f, 0f);
        //Debug.DrawRay((feetSpot.position - offset), Vector3.right, Color.red, 0);
        isOnGround = Physics2D.OverlapArea(feetSpot.position - offset, feetSpot.position + offset, platformLayers);
        if (isOnGround)
        {
            remainingJumpCoyoteTime = movementData.jumpCoyoteTime;
            rb.gravityScale = movementData.gravityScale;
        }
        else
        {
            isJumping = rb.velocity.y > 0f;
        }
    }

    void Jump()
    {
        // If the player is holding the jump button, make a normal jump, else make a short jump
        float trueJumpForce = Input.GetKey(KeyCode.Space) ? movementData.jumpForce : movementData.miniJumpForce;
        rb.velocity = new Vector2(rb.velocity.x, trueJumpForce);
        remainingJumpBufferTime = 0f;
        remainingJumpCoyoteTime = 0f;
    }

    public void DisableAndHide()
    {
        canMove = false;
        sprite.enabled = false;
        rb.velocity = Vector2.zero;
        rb.simulated = false;
    }

    public void EnableAndShow()
    {
        canMove = true;
        sprite.enabled = true;
        rb.simulated = true;
    }

    public void Die()
    {
        canMove = false;
        sprite.color = Color.red;
    }
}
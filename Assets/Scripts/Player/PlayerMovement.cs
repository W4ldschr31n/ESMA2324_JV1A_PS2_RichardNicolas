using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject rig;
    private Animator animator;
    private Recorder recorder;
    [SerializeField] private Transform feetSpot, headSpot;
    [SerializeField] private LayerMask platformLayers;
    [SerializeField] private PlayerMovementData movementData;
    public GameObject gravePrefab;
    private GameObject graveInstance;

    public UnityEvent onPlayerDeath;
    private Rigidbody2D movingPlatformRb;


    private float directionInput;
    public float cancelChargeTime;
    private float remainingJumpBufferTime, remainingJumpCoyoteTime, currentCancelChargedTime;
    public bool isOnGround, isJumping, isDead, isFlipped;
    private bool isRecording;
    public bool canMove;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        recorder = GetComponent<Recorder>();

        if(onPlayerDeath == null)
        {
            onPlayerDeath = new UnityEvent();
        }
    }

    private void Start()
    {
        // Do this in Start to let the event be initialized in an Awake
        TimerManager.onTimerEnded.AddListener(Die);
    }

    private void OnDisable()
    {
        TimerManager.onTimerEnded.RemoveListener(Die);
    }


    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return;
        }
        //Movement

        directionInput = SingletonMaster.Instance.InputManager.MoveInput;

        // Player want to jump
        if (SingletonMaster.Instance.InputManager.JumpInputPressed)
            remainingJumpBufferTime = movementData.jumpBufferTime;
        // Player wants to abort the jump
        else if (SingletonMaster.Instance.InputManager.JumpInputReleased && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        }

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
        // Cancel charge
        if (SingletonMaster.Instance.InputManager.CancelInputPressed || SingletonMaster.Instance.InputManager.CancelInputReleased)
        {
            currentCancelChargedTime = 0f;
        }
        else if (SingletonMaster.Instance.InputManager.CancelInputPressing)
        {
            currentCancelChargedTime += Time.deltaTime;
            if(currentCancelChargedTime >= cancelChargeTime)
            {
                Die();
                currentCancelChargedTime = 0f;
            }
        }
        // Animation
        animator.SetBool("Moving", directionInput != 0f && Mathf.Abs(rb.velocity.x) >= 0.1f);
        animator.SetFloat("VelocityY", rb.velocity.y);
        animator.SetBool("OnGround", isOnGround);
        Flip();
    }

    public void Flip()
    {
        if (directionInput != 0f)
        {
            isFlipped = directionInput < 0f;
            int scale = isFlipped ? -1 : 1;
            rig.transform.localScale = new Vector3(scale * Mathf.Abs(rig.transform.localScale.x), rig.transform.localScale.y, rig.transform.localScale.z);
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
            ReplayData data = new ReplayData(transform.position, isJumping, isFlipped, isDead);
            recorder.RecordReplayData(data);
            // We don't need further data when the player dies
            if (isDead)
            {
                isRecording = false;
            }
        }
    }
    private void FixedUpdate()
    {
        CheckIsOnGround();

        // Check if player wanted to jump recently and were on ground recently
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

        // Add a force corresponding to the platform moving us
        if (movingPlatformRb != null)
        {
            rb.AddForce(Vector2.right * movingPlatformRb.velocity.x * 20); // 20 is amount of FixedUpdate/second
        }
    }

    public void SnapToMovingPlatform(Rigidbody2D _movingPlatformRb)
    {
        movingPlatformRb = _movingPlatformRb;
    }

    public void DetachFromMovingPlatform()
    {
        movingPlatformRb = null;
    }

    private void CheckIsOnGround()
    {
        Vector3 offset = new Vector3(0.2f, 0f, 0f);
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

    private void Jump()
    {
        // If the player is holding the jump button, make a normal jump, else make a short jump
        float trueJumpForce = SingletonMaster.Instance.InputManager.JumpInputPressing ? movementData.jumpForce : movementData.miniJumpForce;
        rb.velocity = new Vector2(rb.velocity.x, trueJumpForce);
        remainingJumpBufferTime = 0f;
        remainingJumpCoyoteTime = 0f;
        animator.SetTrigger("Jump");
    }

    private void DisableBody()
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        rb.simulated = false;
    }

    public void DisableAndHide()
    {
        DisableBody();
        rig.SetActive(false);
    }

    public void EnableAndShow()
    {
        canMove = true;
        rig.SetActive(true);
        rb.simulated = true;
    }

    public void Die()
    {
        // Check if we're not already dead
        if (isDead)
            return;

        DisableBody();
        isDead = true;
        animator.SetTrigger("Die");
        animator.ResetTrigger("Jump");
    }

    public void OnDeathAnimationEnd()
    {
        graveInstance = Instantiate(gravePrefab, headSpot.position, Quaternion.identity);
        DisableAndHide();
        onPlayerDeath?.Invoke();
    }

    public void Resurrect()
    {
        if(graveInstance != null)
        {
            Destroy(graveInstance);
            graveInstance = null;
        }
        EnableAndShow();
        isDead = false;
        animator.SetTrigger("Resurrect");
    }
}

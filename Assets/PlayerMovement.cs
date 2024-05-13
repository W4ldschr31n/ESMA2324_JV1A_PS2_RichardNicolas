using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D hitbox;
    [SerializeField] private Transform feetSpot, headSpot;
    [SerializeField] private LayerMask platformLayers;

    [SerializeField] private float acceleration, groundFriction, airControl, speed, jumpHeight, miniJumpHeight;
    private float directionInput;
    [SerializeField] private float jumpBufferTime, jumpCoyoteTime;
    private float remainingJumpBufferTime, remainingJumpCoyoteTime;

    public bool isOnGround;
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
            remainingJumpBufferTime = jumpBufferTime;
        // Abort jump TODO move velocity part to fixed when inputs are externalised
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
        // Reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.position = Vector2.zero;
            rb.velocity = Vector2.zero;
        }
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
        if (remainingJumpCoyoteTime > 0f && remainingJumpBufferTime > 0f)
        {
            Jump();
        }


        float newXSpeed = Mathf.MoveTowards(rb.velocity.x, speed * directionInput, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(newXSpeed, rb.velocity.y);
    }

    void CheckIsOnGround()
    {
        Vector3 offset = new Vector3(0.5f, 0f, 0f);
        Debug.DrawRay((feetSpot.position - offset), Vector3.right, Color.red, 0);
        isOnGround = Physics2D.OverlapArea(feetSpot.position - offset, feetSpot.position + offset, platformLayers);
        if (isOnGround)
        {
            remainingJumpCoyoteTime = jumpCoyoteTime;
        }
    }

    void Jump()
    {
        float trueJumpHeight = Input.GetKey(KeyCode.Space) ? jumpHeight : miniJumpHeight;
        float jumpForce = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * trueJumpHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        remainingJumpBufferTime = 0f;
        remainingJumpCoyoteTime = 0f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D hitbox;
    [SerializeField] private Transform feetSpot, headSpot;
    [SerializeField] private LayerMask platformLayers;

    [SerializeField] private float speed, jumpHeight;

    [SerializeField] private float jumpBufferTime, jumpCoyoteTime;
    private float remainingJumpBufferTime, remainingJumpCoyoteTime;

    public bool isOnGround;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitbox = GetComponent<Collider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float directionInput = Input.GetAxisRaw("Horizontal");
        rb.AddForce(Vector2.right * directionInput * speed);
        CheckIsOnGround();
        if (Input.GetKeyDown(KeyCode.Space))
            remainingJumpBufferTime = jumpBufferTime;
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y >  0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);
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
    }

    private void FixedUpdate()
    {
        if(remainingJumpCoyoteTime > 0f && remainingJumpBufferTime > 0f)
        {
            Jump();
        }
    }

    void CheckIsOnGround()
    {
        Vector3 offset = new Vector3(0.5f, 0f, 0f);
        Debug.DrawRay((feetSpot.position - offset), Vector3.right, Color.green, 0);
        isOnGround = Physics2D.OverlapArea(feetSpot.position - offset, feetSpot.position + offset, platformLayers);
        if (isOnGround)
        {
            remainingJumpCoyoteTime = jumpCoyoteTime;
        }
    }

    void Jump()
    {
        float jumpForce = Mathf.Sqrt(-2.0f * Physics2D.gravity.y * jumpHeight);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        remainingJumpBufferTime = 0f;
        remainingJumpCoyoteTime = 0f;
    }
}

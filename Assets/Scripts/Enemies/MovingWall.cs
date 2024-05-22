using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    private bool isMoving;
    private Rigidbody2D rb;
    public bool goRight;
    public float acceleration, deceleration;
    public GameObject leftTrigger, rightTrigger;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Enable the trigger zone corresponding to the direction we want
        leftTrigger.SetActive(!goRight);
        rightTrigger.SetActive(goRight);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving)
        { // Constant acceleration
            float direction = goRight ? 1f : -1f;
            float addedSpeed = acceleration * Time.fixedDeltaTime * direction;
            rb.velocity = new Vector2(rb.velocity.x + addedSpeed, 0f);
        }
        else if (rb.velocity.x != 0f)
        { // Slow down until we stop compltetly
            float newSpeed = Mathf.MoveTowards(rb.velocity.x, 0f, deceleration * Time.fixedDeltaTime);
            rb.velocity = new Vector2(newSpeed, 0f);
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        isMoving = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;
    private int currentIndex;
    private Vector2 targetPosition;
    public bool isPingPong;
    private int stepWaypoint;
    public bool isMoving;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Reset();
        isMoving = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isMoving)
        {
            return;
        }

        Vector2 currentPosition = transform.position;

        // Target reached
        if (Vector2.Distance(currentPosition, targetPosition) <= 0.1f)
        {
            // Ping pong : we want to travel back to the previous waypoints
            if (isPingPong)
            {
                if (currentIndex == waypoints.Length - 1)
                {
                    stepWaypoint = -1;
                }
                else if (currentIndex == 0)
                {
                    stepWaypoint = 1;
                }
            }
            // Not ping pong : loop through the waypoints again
            else
            {
                if (currentIndex == waypoints.Length - 1)
                {
                    currentIndex = -1;
                }
                else if (currentIndex == 0)
                {
                    currentIndex = waypoints.Length;
                }
            }
            // Move towards the next waypoint
            currentIndex += stepWaypoint;
            targetPosition = waypoints[currentIndex].position;
            rb.velocity = (targetPosition - currentPosition).normalized * speed;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if we're coming from above (platform effector enables the collision)
        if (collision.enabled && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().SnapToMovingPlatform(rb);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().DetachFromMovingPlatform();
        }
    }

    public void Reset()
    {
        // Move the platform to the first waypoint
        currentIndex = 0;
        targetPosition = waypoints[currentIndex].position;
        transform.position = targetPosition;
        stepWaypoint = 1;
        isMoving = true;
    }
}

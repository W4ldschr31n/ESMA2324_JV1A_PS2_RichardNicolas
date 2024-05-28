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

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            return;
        }

        Vector2 currentPosition = transform.position;
        // Too far away from target, move towards it
        if(Vector2.Distance(currentPosition, targetPosition) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
        else // Target reached
        {
            // Ping pong : we want to travel back to the previous waypoints
            if (isPingPong)
            {
                if(currentIndex == waypoints.Length -1)
                {
                    stepWaypoint = -1;
                }else if(currentIndex == 0)
                {
                    stepWaypoint = 1;
                }
            }
            // Not ping pong : loop through the waypoints again
            else if(currentIndex == waypoints.Length -1)
            {
                currentIndex = -1;
            }
            // Get the next waypoint
            currentIndex += stepWaypoint;
            targetPosition = waypoints[currentIndex].position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.transform.SetParent(null);
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

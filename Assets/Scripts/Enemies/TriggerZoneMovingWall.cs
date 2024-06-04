using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneMovingWall : MonoBehaviour
{
    public MovingWall movingWall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { // Start moving when we see current player
            movingWall.StartMoving();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") && !collision.CompareTag("PlayerGrave"))
        { // Stop moving when we see a wall
            movingWall.StopMoving();
        }
    }
}

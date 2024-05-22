using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneMovingWall : MonoBehaviour
{
    public MovingWall movingWall;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            movingWall.StartMoving();
        }else if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            movingWall.StopMoving();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMovingWall : Resetable
{
    private Vector2 position;
    private Rigidbody2D rb;
    private MovingWall movingWall;

    override protected void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        movingWall = GetComponent<MovingWall>();
    }

    protected override void RestoreData()
    {
        rb.velocity = Vector2.zero;
        transform.position = position;
        movingWall.StopMoving();
    }

    protected override void StoreData()
    {
        position = transform.position;
    }
}

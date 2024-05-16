using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFallingPlatform : Resetable
{
    private Vector2 position;
    private Rigidbody2D rb;

    override protected void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void RestoreData()
    {
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        transform.position = position;
    }

    protected override void StoreData()
    {
        position = transform.position;
    }
}

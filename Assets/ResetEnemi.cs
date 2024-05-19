using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnemi : Resetable
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
        rb.velocity = Vector2.zero;
        transform.position = position;
    }

    protected override void StoreData()
    {
        position = transform.position;
    }
}

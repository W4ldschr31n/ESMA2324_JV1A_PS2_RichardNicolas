using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activable
{
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void Activate()
    {
        rb.simulated = false;
        sprite.enabled = false;
    }

    public override void Deactivate()
    {
        rb.simulated = true;
        sprite.enabled = true;
    }

}

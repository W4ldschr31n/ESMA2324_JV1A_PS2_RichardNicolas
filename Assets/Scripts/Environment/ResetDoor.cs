using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDoor : Resetable
{
    public GameObject body;
    private Vector2 position;

    override protected void Start()
    {
        base.Start();
    }

    protected override void RestoreData()
    {
        body.transform.position = position;
    }

    protected override void StoreData()
    {
        position = body.transform.position;
    }
}

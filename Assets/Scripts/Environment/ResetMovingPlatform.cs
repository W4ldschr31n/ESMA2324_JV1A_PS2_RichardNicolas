using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMovingPlatform : Resetable
{
    private MovingPlatform movingPlatform;

    override protected void Start()
    {
        base.Start();
        movingPlatform = GetComponent<MovingPlatform>();
    }

    protected override void RestoreData()
    {
        movingPlatform.Reset();
    }

    protected override void StoreData()
    {
        return;
    }

}

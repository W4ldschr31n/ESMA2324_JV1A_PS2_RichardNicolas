using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnemyBird : Resetable
{
    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;
    private EnemyBird enemyBird;

    override protected void Start()
    {
        base.Start();
        enemyBird = GetComponent<EnemyBird>();
    }

    protected override void RestoreData()
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
        enemyBird.Reset();

    }

    protected override void StoreData()
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }
}

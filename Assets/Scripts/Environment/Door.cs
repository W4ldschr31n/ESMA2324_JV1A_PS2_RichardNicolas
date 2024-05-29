using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activable
{
    public Transform body, spotOpen, spotClose;
    private Vector3 targetPosition;
    private float speed;
    public float speedOpen, speedClose;

    public override void Activate()
    {
        targetPosition = spotOpen.position;
        speed = speedOpen;
    }

    public override void Deactivate()
    {
        targetPosition = spotClose.position;
        speed = speedClose;
    }

    private void Update()
    {
        Debug.Log(Vector2.Distance(body.position, targetPosition));
        if(Vector2.Distance(body.position, targetPosition) != 0f)
        {
            body.position = Vector3.MoveTowards(body.position, targetPosition, speed * Time.deltaTime);
            Debug.Log(Vector3.MoveTowards(body.position, targetPosition, speed * Time.deltaTime));
        }
    }

}

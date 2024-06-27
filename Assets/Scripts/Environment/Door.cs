using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activable
{
    public Transform body, spotOpen, spotClose;
    private Vector3 targetPosition;
    private float speed;
    public float speedOpen, speedClose;
    public Animator alertAnimator;

    public override void Activate()
    {
        targetPosition = spotOpen.position;
        speed = speedOpen;
        alertAnimator.SetBool("Activated", true);
    }

    public override void Deactivate()
    {
        targetPosition = spotClose.position;
        speed = speedClose;
        alertAnimator.SetBool("Activated", false);
    }

    private void Update()
    {
        if(Vector2.Distance(body.position, targetPosition) != 0f)
        {
            body.position = Vector3.MoveTowards(body.position, targetPosition, speed * Time.deltaTime);
        }
    }

}

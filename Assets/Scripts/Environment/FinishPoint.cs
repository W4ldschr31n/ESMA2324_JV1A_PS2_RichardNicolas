using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private Animator animator;
    private bool isMoving;
    public float speed = 4;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(speed * Vector2.up * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            SingletonMaster.Instance.GameManager.FinishGame(transform.position);
    }

    public void Close()
    {
        animator.Play("ElevatorClose");
        Invoke(nameof(Move), 0.5f);
    }

    private void Move()
    {
        isMoving = true;
    }
}

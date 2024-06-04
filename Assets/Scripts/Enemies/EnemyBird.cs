using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : MonoBehaviour
{
    public LayerMask playerLayer;
    private float speed;
    public float maxSpeed;
    public float accelerationLockingPhase;
    public float accelerationChasePhase;
    public float speedChaseThreshold;
    private BirdState currentState;
    private Vector3 direction;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Reset();
    }

    // Update is called once per frame
    private void Update()
    {
        // Locking : turn to target
        if(currentState == BirdState.Locking)
        {
            transform.up = direction;
        }
    }
    void FixedUpdate()
    {
        // Idle : search for target (current player only)
        if(currentState == BirdState.Idle)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, playerLayer);
            if (hit && hit.transform.CompareTag("Player"))
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                direction = (hit.transform.position - transform.position).normalized;
                currentState = BirdState.Locking;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.up * 20f, Color.green);
            }
        }
        // Locking : aim for target and accelerate slowly until threshold
        else if(currentState == BirdState.Locking) 
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, playerLayer);
            if (hit && hit.transform.CompareTag("Player"))
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                direction = (hit.transform.position - transform.position).normalized;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.up * 20f, Color.green);
            }
            speed = Mathf.Min(speed + accelerationLockingPhase * Time.fixedDeltaTime, speedChaseThreshold);
            transform.position += direction * speed * Time.fixedDeltaTime;
            if(speed >= speedChaseThreshold)
            {
                currentState = BirdState.Chasing;
            }
        }
        // Chasing : accelerate fully toward last target position
        else if (currentState == BirdState.Chasing)
        {
            Debug.DrawRay(transform.position, transform.up * 20f, Color.green);
            speed = Mathf.Min(speed + accelerationLockingPhase * Time.fixedDeltaTime, maxSpeed);
            transform.position += direction * speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we hit the current player, kill them and destroy self
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().Die();
            DestroySelf();
        } else if(collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        { // If we hit the ground, or a replay, destroy self
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        rb.simulated = false;
        sprite.enabled = false;
        Debug.Log("prrt");
    }

    public void Reset()
    {
        speed = 0f;
        currentState = BirdState.Idle;
        sprite.enabled = true;
        rb.simulated = true;
    }
}

public enum BirdState
{
    Idle,
    Locking,
    Chasing,
}

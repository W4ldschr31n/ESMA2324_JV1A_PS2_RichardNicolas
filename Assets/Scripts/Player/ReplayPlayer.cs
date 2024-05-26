using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject gravePrefab;
    private GameObject graveInstance;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        TimerManager.onTimerStarted.AddListener(Resurrect);
    }

    private void OnDisable()
    {
        TimerManager.onTimerStarted.RemoveListener(Resurrect);
    }

    public void SetReplayData(ReplayData data)
    {
        transform.position = data.position;
        // Sometimes the sprite variable is not yet initialized when the first update calls this
        if(sprite != null)
        {
            sprite.flipX = data.isFlippedX;
        }
        if (data.isDead)
        {
            Die();
        }
        // todo animation
    }

    private void Die()
    {
        // Make the body disappear and spawn a grave instead
        sprite.enabled = false;
        rb.simulated = false;
        graveInstance = Instantiate(gravePrefab, transform.position, Quaternion.identity);
    }

    public void Resurrect()
    {
        if(graveInstance != null)
        {
            Destroy(graveInstance);
            graveInstance = null;
        }

        sprite.enabled = true;
        rb.simulated = true;
    }

    public void DestroySelf()
    {
        // Clean up before destroying
        if (graveInstance != null)
        {
            Destroy(graveInstance);
            graveInstance = null;
        }
        Destroy(gameObject);
    }
}

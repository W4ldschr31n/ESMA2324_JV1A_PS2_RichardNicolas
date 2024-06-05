using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float timeToLive;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Shoot the bullet in the direction it has spawned
        rb.velocity = transform.right * speed;
        // Automatically destroy itself after timeToLive seconds
        Invoke(nameof(SelfDestroy), timeToLive);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().Die();
            SelfDestroy();
        }else if (collision.gameObject.layer == LayerMask.NameToLayer("Platform") && !collision.gameObject.CompareTag("PressButton"))
        {
            SelfDestroy();
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}

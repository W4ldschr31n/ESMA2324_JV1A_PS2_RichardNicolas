using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    public GameObject activable;
    public LayerMask playerLayer;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        if (rb.IsTouchingLayers(playerLayer))
        {
            activable.SetActive(false);
        }
        else
        {
            activable.SetActive(true);
        }
    }
}

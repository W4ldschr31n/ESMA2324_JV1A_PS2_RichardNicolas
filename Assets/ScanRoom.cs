using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanRoom : MonoBehaviour
{
    public int nbPlayersNeeded;
    public GameObject activable;
    private Rigidbody2D rb;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ContactFilter2D playerFilter = new ContactFilter2D();
        playerFilter.SetLayerMask(playerLayer);
        List<Collider2D> playersHere = new();
        rb.OverlapCollider(playerFilter, playersHere);
        if(playersHere.Count >= nbPlayersNeeded)
        {
            activable.SetActive(false);
        }
        else
        {
            activable.SetActive(true);
        }
    }
}

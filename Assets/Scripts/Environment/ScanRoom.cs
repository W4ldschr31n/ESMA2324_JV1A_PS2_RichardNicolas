using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScanRoom : Activator
{
    public int nbPlayersNeeded;
    private Rigidbody2D rb;
    public LayerMask playerLayer;
    public TextMeshProUGUI text;
    private ContactFilter2D playerFilter;
    private List<Collider2D> playersHere;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerFilter = new ContactFilter2D();
        playerFilter.SetLayerMask(playerLayer);
        playersHere = new();
    }


    override protected bool CheckCanActivate()
    {
        rb.OverlapCollider(playerFilter, playersHere);
        return playersHere.Count >= nbPlayersNeeded;
    }

    override protected void UpdateDisplay()
    {
        text.text = $"{playersHere.Count} / {nbPlayersNeeded}";
        if(playersHere.Count <= 0)
        {
            text.color = Color.red;
        }else if(playersHere.Count == nbPlayersNeeded)
        {
            text.color = Color.green;
        }
        else
        {
            text.color = Color.yellow;
        }
    }
}

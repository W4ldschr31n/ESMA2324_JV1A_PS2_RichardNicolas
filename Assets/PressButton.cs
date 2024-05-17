using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : Activator
{
    public LayerMask playerLayer;
    private ContactFilter2D playerFilter;
    [SerializeField] private Collider2D pressSpot;

    // Start is called before the first frame update
    void Start()
    {
        playerFilter = new ContactFilter2D();
        playerFilter.SetLayerMask(playerLayer);
    }

    protected override bool CheckCanActivate()
    {
        List<Collider2D> playersHere = new();
        pressSpot.OverlapCollider(playerFilter, playersHere);
        return playersHere.Count > 0;
    }

    protected override void UpdateDisplay()
    {
        return;
    }
}

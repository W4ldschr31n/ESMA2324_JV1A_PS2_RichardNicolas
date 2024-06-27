using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : Activator
{
    public LayerMask playerLayer;
    private ContactFilter2D playerFilter;
    private Animator animator;
    [SerializeField] private Collider2D pressSpot;
    public Animator alertAnimator;

    // Start is called before the first frame update
    void Start()
    {
        playerFilter = new ContactFilter2D();
        playerFilter.SetLayerMask(playerLayer);
        animator = GetComponent<Animator>();
    }

    protected override bool CheckCanActivate()
    {
        List<Collider2D> playersHere = new();
        pressSpot.OverlapCollider(playerFilter, playersHere);
        bool activated = playersHere.Count > 0;
        animator.SetBool("Down", activated);
        alertAnimator.SetBool("Activated", activated);
        return activated;
    }

    protected override void UpdateDisplay()
    {
        return;
    }
}

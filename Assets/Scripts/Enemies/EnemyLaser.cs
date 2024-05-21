using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float delayBeforeFirstShot;
    public float delayBetweenShots;
    public float maxRange;
    private bool isShooting;
    private LineRenderer lineRenderer;
    [SerializeField] private Transform laserStart;
    private Vector2 laserEndPosition;
    private Vector2 maxPosition;
    private Vector2 laserOffset;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserEndPosition = maxPosition = laserStart.position + transform.up * maxRange;
        lineRenderer.SetPosition(0, laserStart.position);
        lineRenderer.SetPosition(1, laserEndPosition);
        TimerManager.onTimerStarted.AddListener(OnTimerStarted);
        TimerManager.onTimerEnded.AddListener(OnTimerEnded);
        // Controls how the collisions will be detected around the laser
        laserOffset = new Vector2(0.5f, 0f);
    }

    private void OnDisable()
    {
        TimerManager.onTimerStarted.RemoveListener(OnTimerStarted);
        TimerManager.onTimerEnded.RemoveListener(OnTimerEnded);
    }

    private void Update()
    {
        lineRenderer.SetPosition(1, laserEndPosition);
    }

    private void FixedUpdate()
    {
        // Check how far the laser should go
        RaycastHit2D platformHit = Physics2D.Raycast(laserStart.position, transform.up, maxRange, LayerMask.NameToLayer("Platform"));
        if (platformHit)
        {
            laserEndPosition = platformHit.point;
        }
        else
        {
            laserEndPosition = maxPosition;
        }
        // Check if there is a player caught in the laser
        if (isShooting)
        {
            Collider2D[] playerHit = Physics2D.OverlapAreaAll((Vector2)laserStart.position + laserOffset, laserEndPosition - laserOffset);
            foreach(Collider2D player in playerHit)
            { // If the current player is hit, kill them
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<PlayerMovement>().Die();
                }
            }
        }
    }

    private void OnTimerStarted()
    {
        isShooting = false;
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;
        lineRenderer.widthMultiplier = 0.1f;
        // Make sure we have no overlapping shots
        CancelInvoke();
        Invoke(nameof(InitiateShot), delayBeforeFirstShot);
    }

    private void InitiateShot()
    {
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.widthMultiplier = 0.2f;
        Invoke(nameof(FireShot), 0.5f);
    }

    private void FireShot()
    {
        isShooting = true;
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.widthMultiplier = 1f;
        Invoke(nameof(ShutdownShot), 0.5f);
    }

    private void ShutdownShot()
    {
        isShooting = false;
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;
        lineRenderer.widthMultiplier = 0.1f;
        Invoke(nameof(InitiateShot), delayBetweenShots);
    }

    private void OnTimerEnded()
    {
    }
}

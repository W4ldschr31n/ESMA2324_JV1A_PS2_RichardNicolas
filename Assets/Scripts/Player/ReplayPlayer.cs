using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayPlayer : MonoBehaviour
{
    [SerializeField] private GameObject rig;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    public GameObject gravePrefab;
    private GameObject graveInstance;

    private void Start()
    {
        TimerManager.onTimerStarted.AddListener(Resurrect);
    }

    private void OnDisable()
    {
        TimerManager.onTimerStarted.RemoveListener(Resurrect);
    }

    public void SetReplayData(ReplayData data)
    {
        transform.position = data.position;
        int scale = data.isFlippedX ? -1 : 1;
        rig.transform.localScale = new Vector3(scale * Mathf.Abs(rig.transform.localScale.x), rig.transform.localScale.y, rig.transform.localScale.z);
        animator.SetBool("OnGround", data.isOnGround);
        animator.SetBool("Moving", data.isMoving);
        if (data.isDead)
        {
            Die();
        }
    }

    public void Hide()
    {
        rig.SetActive(false);
    }

    private void Die()
    {
        // Make the body disappear and spawn a grave instead
        rig.SetActive(false);
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

        rig.SetActive(true);
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

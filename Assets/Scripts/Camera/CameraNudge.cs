using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNudge : MonoBehaviour
{
    public Transform nudgePosition;
    public float nudgeDuration;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SingletonMaster.Instance.CameraManager.Nudge(nudgePosition, nudgeDuration);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SingletonMaster.Instance.CameraManager.EndNudge();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNudge : MonoBehaviour
{
    public Transform nudgePosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SingletonMaster.Instance.CameraManager.Nudge(nudgePosition);
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

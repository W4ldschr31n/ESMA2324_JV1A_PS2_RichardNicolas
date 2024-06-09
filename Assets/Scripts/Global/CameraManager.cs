using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera _camera;
    private CameraFollow cameraFollow;
    private CameraZoom cameraZoom;

    private void Awake()
    {
        cameraFollow = _camera.GetComponent<CameraFollow>();
        cameraZoom = _camera.GetComponent<CameraZoom>();
    }

    public void SetCameraTarget(Transform target)
    {
        cameraFollow.SetTarget(target);
    }

    public void ForcePosition(Vector3 position)
    {
        cameraFollow.transform.position = new Vector3(position.x, position.y, cameraFollow.transform.position.z);
    }

    public void ResetZoom()
    {
        cameraZoom.ResetSize();
    }

    public void ZoomIn()
    {
        cameraZoom.ZoomIn();
    }

    public void ZoomOut()
    {
        cameraZoom.ZoomOut();
    }

    public void Nudge(Transform nudgeTarget)
    {
        cameraZoom.ZoomOut();
        cameraFollow.Nudge(nudgeTarget);
    }

    public void EndNudge()
    {
        cameraZoom.ResetSize();
        cameraFollow.EndNudge();
    }
}

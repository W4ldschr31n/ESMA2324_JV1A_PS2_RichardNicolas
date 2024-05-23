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

    public void Nudge(Transform nudgeTarget, float nudgeDuration)
    {
        cameraZoom.ZoomOut();
        cameraFollow.Nudge(nudgeTarget);
        Invoke(nameof(EndNudge), nudgeDuration);
    }

    public void EndNudge()
    {
        cameraZoom.ResetSize();
        cameraFollow.EndNudge();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Camera _camera;
    public float baseSize;
    public float zoomInSize;
    public float zoomOutSize;
    private float targetSize;
    private float stepSize = 1;
    public float timeToZoom;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        targetSize = baseSize = _camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float newSize = Mathf.MoveTowards(_camera.orthographicSize, targetSize, stepSize * Time.deltaTime);
        _camera.orthographicSize = newSize;
    }

    public void ZoomIn()
    {
        ChangeSize(zoomInSize);
    }

    public void ZoomOut()
    {
        ChangeSize(zoomOutSize);
    }

    public void ChangeSize(float newSize)
    {
        targetSize = newSize;
        stepSize = Mathf.Abs(targetSize - _camera.orthographicSize)/timeToZoom;
    }

    public void ResetSize()
    {
        ChangeSize(baseSize);
    }
}
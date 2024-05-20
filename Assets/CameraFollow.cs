using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Camera _camera;
	public float smoothTime = 1f;
	private Vector3 velocity;
	public float lookAheadOffsetX;
	public float lookAheadOffsetY;
	private Rigidbody2D targetRb;


	private void Start()
    {
        _camera = GetComponent<Camera>();
		targetRb = target.GetComponent<Rigidbody2D>();
		velocity = Vector3.zero;
    }

    void Update()
	{
		if (target)
		{
			Vector3 from = _camera.transform.position;
			Vector3 to = target.position;
			// Keep original z else camera cannot display anything
			if (targetRb != null)
            {
				// Math lib is used to get 0 when velocity is 0
				float offsetX = Math.Sign(targetRb.velocity.x) * lookAheadOffsetX;
				float offsetY = Math.Sign(targetRb.velocity.y) * lookAheadOffsetY;
				to.x += offsetX;
				to.y += offsetY;
            }
			to.z = from.z;

			_camera.transform.position = Vector3.SmoothDamp(from, to, ref velocity, smoothTime);
		}
	}
}

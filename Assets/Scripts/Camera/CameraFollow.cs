using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Transform nudgeTarget;
	private bool isNudging;
    private Camera _camera;
	public float smoothTime = 1f;
	private Vector3 velocity;
	public float lookAheadOffsetX;
	public float offsetY;
	private Rigidbody2D targetRb;


	private void Start()
    {
        _camera = GetComponent<Camera>();
		velocity = Vector3.zero;
    }

	public void SetTarget(Transform _target)
	{
		target = _target;
        targetRb = target.GetComponent<Rigidbody2D>();
    }

    void Update()
	{
		if (target != null)
		{
			Vector3 from = _camera.transform.position;
			Vector3 to = isNudging ? nudgeTarget.position : target.position;
			// Keep original z else camera cannot display anything
			if (targetRb != null)
            {
				float offsetX = 0f;
				if(Mathf.Abs(targetRb.velocity.x) > 0.1f)
                {
					offsetX = Mathf.Sign(targetRb.velocity.x) * lookAheadOffsetX;
				}
				to.x += offsetX;
				to.y += offsetY;
            }
			to.z = from.z;

			_camera.transform.position = Vector3.SmoothDamp(from, to, ref velocity, smoothTime);
		}
	}

	public void Nudge(Transform nudgePosition)
	{
		isNudging = true;
		nudgeTarget = nudgePosition;
	}

	public void EndNudge()
	{
		isNudging = false;
		nudgeTarget = null;
	}
}

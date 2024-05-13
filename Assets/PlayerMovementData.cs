using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Movement")]
    public float baseAcceleration;
    public float deceleration;
    public float airControl;
    public float baseMaxSpeed;

    [Header("Jumping")]
    public float jumpHeight;
    public float miniJumpHeight;
    public float fallingGravityScale;
    public float maxFallingSpeed;

    [Header("Accessibility")]
    public float jumpBufferTime;
    public float jumpCoyoteTime;
    public float apexBoostThreshold;
    public float apexBoostMaxSpeed;
    public float apexBoostAcceleration;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    [Header("Movement")]
    public float baseAcceleration; // Acceleration on the ground (moving)
    public float deceleration; // Deceleration on the ground (stopping)
    public float airControl; // Acceleration in air (moving)
    public float baseMaxSpeed;

    [Header("Jumping")]
    public float jumpHeight; // How high (in grid units) we want to jump at maximum
    public float miniJumpRatio; // How high (relative to max jump) we want to jump at minimum
    public float jumpApexTime; // How long should it take to reach the apex of the jump
    [HideInInspector] public float jumpForce; // The computed force needed to reach jumpHeight within jumpApexTime
    [HideInInspector] public float miniJumpForce; // The computed force needed to reach jumpHeight * miniJumpRatio

    [Header("Gravity")]
    public float fallingGravityScale; // How much faster we go down than we go up
    public float maxFallingSpeed; // Clamps the falling speed to avoid loss of control
    [HideInInspector] public float gravityScale; // The computed gravity force needed to compensate the jumpForce

    [Header("Accessibility")]
    public float jumpBufferTime;
    public float jumpCoyoteTime;
    public float apexBoostThreshold;
    public float apexBoostMaxSpeed;
    public float apexBoostAcceleration;
    public float apexBoostGravityMult;

    // Update hidden values when changes occur in inspector
    private void OnValidate()
    {
        // Formulas are courtesy of https://github.com/DawnosaurDev/platformer-movement
        // Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        float gravityStrength = -2 * jumpHeight / Mathf.Pow(jumpApexTime, 2);

        // Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        // Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = -gravityStrength * jumpApexTime;
        miniJumpForce = jumpForce * miniJumpRatio;
    }
}

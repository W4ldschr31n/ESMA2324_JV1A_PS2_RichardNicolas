using UnityEngine;

public struct ReplayData
{
    public Vector3 position;
    public bool isOnGround;
    public bool isMoving;
    public bool isFlippedX;
    public bool isDead;

    public ReplayData(Vector3 _position, bool _isOnGround, bool _isMoving, bool _isFlippedX, bool _isDead)
    {
        position = _position;
        isOnGround = _isOnGround;
        isMoving = _isMoving;
        isFlippedX = _isFlippedX;
        isDead = _isDead;

    }
}
